using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.Web.ViewModels.Barbers;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZentrixBarber.Data.Models;

namespace ZentrixBarber.Services
{
    public class BarberService : IBarberService
    {
        private readonly ApplicationDbContext dbContext;


        public BarberService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task<IEnumerable<BarberIndexViewModel>> GetActiveBarbersAsync()
        {
            return await dbContext.Barbers
                .AsNoTracking()
                .Where(b => b.IsActive)
                .OrderBy(b => b.FullName)
                .Select(b => new BarberIndexViewModel
                {
                    Id = b.Id,
                    FullName = b.FullName,
                    ImageUrl = b.ImageUrl,
                    YearsOfExperience = b.YearsOfExperience,
                    TreatmentsCount = b.BarberTreatments.Count
                })
                .ToListAsync();
        }

        public async Task<BarberDetailsViewModel?> GetBarberDetailsAsync(int id)
        {
            return await dbContext.Barbers

                .AsNoTracking()
                .Where(b => b.Id == id && b.IsActive)
                .Select(b => new BarberDetailsViewModel
                {
                    Id = b.Id,
                    FullName = b.FullName,
                    Bio = b.Bio,
                    ImageUrl = b.ImageUrl,
                    YearsOfExperience = b.YearsOfExperience,
                    Treatments = b.BarberTreatments
                        .Select(bt => new BarberTreatmentViewModel
                        {
                            Name = bt.Treatment.Name,
                            Price = bt.Treatment.Price,
                            DurationMinutes = bt.Treatment.DurationMinutes
                        })
                        .OrderBy(t => t.Name)
                        .ToList()
                })
                .FirstOrDefaultAsync();


        }


        public async Task<IEnumerable<AdminBarberIndexViewModel>> GetAllBarbersAsync()
        {
            return await dbContext.Barbers
                .AsNoTracking()
                .OrderByDescending(b => b.IsActive)
                .ThenBy(b => b.FullName)
                .Select(b => new AdminBarberIndexViewModel
                {
                    Id = b.Id,
                    FullName = b.FullName,
                    YearsOfExperience = b.YearsOfExperience,
                    TreatmentsCount = b.BarberTreatments.Count,
                    UpcomingAppointmentsCount = b.Appointments
                        .Count(a => a.StartTime >= DateTime.Today),
                    IsActive = b.IsActive
                })
                .ToListAsync();
        }

        public async Task<BarberFormViewModel?> GetBarberForEditAsync(int id)
        {
            BarberFormViewModel? model = await dbContext.Barbers
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new BarberFormViewModel
                {
                    FullName = b.FullName,
                    Bio = b.Bio,
                    ImageUrl = b.ImageUrl,
                    YearsOfExperience = b.YearsOfExperience,
                    SelectedTreatmentIds = b.BarberTreatments
                        .Select(bt => bt.TreatmentId)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (model != null)
            {
                await PopulateTreatmentsAsync(model);
            }

            return model;
        }

        public async Task PopulateTreatmentsAsync(BarberFormViewModel model)
        {
            model.Treatments = await dbContext.Treatments
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToListAsync();
        }

        public async Task<int> CreateBarberAsync(BarberFormViewModel model)
        {
            Barber barber = new Barber
            {
                FullName = model.FullName.Trim(),
                Bio = model.Bio?.Trim(),
                ImageUrl = model.ImageUrl?.Trim(),
                YearsOfExperience = model.YearsOfExperience,
                IsActive = true
            };

            foreach (int treatmentId in await GetExistingTreatmentIdsAsync(model.SelectedTreatmentIds))
            {
                barber.BarberTreatments.Add(new BarberTreatment { TreatmentId = treatmentId });
            }

            await dbContext.Barbers.AddAsync(barber);
            await dbContext.SaveChangesAsync();

            return barber.Id;
        }

        public async Task<bool> UpdateBarberAsync(int id, BarberFormViewModel model)
        {
            Barber? barber = await dbContext.Barbers
                .Include(b => b.BarberTreatments)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (barber == null)
            {
                return false;
            }

            barber.FullName = model.FullName.Trim();
            barber.Bio = model.Bio?.Trim();
            barber.ImageUrl = model.ImageUrl?.Trim();
            barber.YearsOfExperience = model.YearsOfExperience;

            List<int> selectedIds = await GetExistingTreatmentIdsAsync(model.SelectedTreatmentIds);

            List<BarberTreatment> linksToRemove = barber.BarberTreatments
                .Where(bt => !selectedIds.Contains(bt.TreatmentId))
                .ToList();

            foreach (BarberTreatment link in linksToRemove)
            {
                barber.BarberTreatments.Remove(link);
            }

            foreach (int treatmentId in selectedIds)
            {
                if (!barber.BarberTreatments.Any(bt => bt.TreatmentId == treatmentId))
                {
                    barber.BarberTreatments.Add(new BarberTreatment { TreatmentId = treatmentId });
                }
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool?> ToggleActiveAsync(int id)
        {
            Barber? barber = await dbContext.Barbers
                .FirstOrDefaultAsync(b => b.Id == id);

            if (barber == null)
            {
                return null;
            }

            barber.IsActive = !barber.IsActive;
            await dbContext.SaveChangesAsync();

            return barber.IsActive;
        }

        private async Task<List<int>> GetExistingTreatmentIdsAsync(IEnumerable<int> treatmentIds)
        {
            List<int> distinctIds = treatmentIds.Distinct().ToList();

            return await dbContext.Treatments
                .Where(t => distinctIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync();
        }

    }

}
