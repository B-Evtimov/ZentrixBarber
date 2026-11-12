using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.ViewModels.Barbers;

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
    }

}
