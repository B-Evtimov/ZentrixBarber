using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.Web.ViewModels.Treatments;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZentrixBarber.Data.Models;

namespace ZentrixBarber.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ApplicationDbContext dbContext;

        public TreatmentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TreatmentCategoryViewModel>> GetTreatmentsByCategoryAsync()
        {
            return await dbContext.TreatmentCategories
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .Select(c => new TreatmentCategoryViewModel
                {
                    Name = c.Name,
                    Description = c.Description,
                    Treatments = c.Treatments
                        .OrderBy(t => t.Price)
                        .Select(t => new TreatmentCardViewModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                            Price = t.Price,
                            DurationMinutes = t.DurationMinutes
                        })
                        .ToList()
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<AdminTreatmentIndexViewModel>> GetAllTreatmentsAsync()
        {
            return await dbContext.Treatments
                .AsNoTracking()
                .OrderBy(t => t.Category.Name)
                .ThenBy(t => t.Name)
                .Select(t => new AdminTreatmentIndexViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    Price = t.Price,
                    DurationMinutes = t.DurationMinutes,
                    BarbersCount = t.BarberTreatments.Count,
                    AppointmentsCount = t.Appointments.Count
                })
                .ToListAsync();
        }

        public async Task<AdminTreatmentIndexViewModel?> GetTreatmentSummaryAsync(int id)
        {
            return await dbContext.Treatments
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new AdminTreatmentIndexViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    Price = t.Price,
                    DurationMinutes = t.DurationMinutes,
                    BarbersCount = t.BarberTreatments.Count,
                    AppointmentsCount = t.Appointments.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TreatmentFormViewModel?> GetTreatmentForEditAsync(int id)
        {
            TreatmentFormViewModel? model = await dbContext.Treatments
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TreatmentFormViewModel
                {
                    Name = t.Name,
                    Description = t.Description,
                    Price = t.Price,
                    DurationMinutes = t.DurationMinutes,
                    CategoryId = t.CategoryId
                })
                .FirstOrDefaultAsync();

            if (model != null)
            {
                await PopulateCategoriesAsync(model);
            }

            return model;
        }

        public async Task PopulateCategoriesAsync(TreatmentFormViewModel model)
        {
            model.Categories = await dbContext.TreatmentCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await dbContext.TreatmentCategories
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task CreateTreatmentAsync(TreatmentFormViewModel model)
        {
            Treatment treatment = new Treatment
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim(),
                Price = model.Price,
                DurationMinutes = model.DurationMinutes,
                CategoryId = model.CategoryId
            };

            await dbContext.Treatments.AddAsync(treatment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateTreatmentAsync(int id, TreatmentFormViewModel model)
        {
            Treatment? treatment = await dbContext.Treatments
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treatment == null)
            {
                return false;
            }

            treatment.Name = model.Name.Trim();
            treatment.Description = model.Description?.Trim();
            treatment.Price = model.Price;
            treatment.DurationMinutes = model.DurationMinutes;
            treatment.CategoryId = model.CategoryId;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasAppointmentsAsync(int id)
        {
            return await dbContext.Appointments
                .AnyAsync(a => a.TreatmentId == id);
        }

        public async Task<bool> DeleteTreatmentAsync(int id)
        {
            Treatment? treatment = await dbContext.Treatments
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treatment == null)
            {
                return false;
            }

            dbContext.Treatments.Remove(treatment);
            await dbContext.SaveChangesAsync();

            return true;
        }

    }
}
