using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.ViewModels.Treatments;

namespace ZentrixBarber.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TreatmentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            IEnumerable<TreatmentCategoryViewModel> categories = await dbContext.TreatmentCategories
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

            return View(categories);

        }


    }
}