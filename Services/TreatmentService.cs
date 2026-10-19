using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.ViewModels.Treatments;

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

    }
}
