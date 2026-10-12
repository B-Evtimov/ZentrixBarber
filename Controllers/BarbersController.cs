using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Data;
using ZentrixBarber.ViewModels.Barbers;
using Microsoft.EntityFrameworkCore;

namespace ZentrixBarber.Controllers
{
    public class BarbersController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public BarbersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BarberIndexViewModel> barbers = await dbContext.Barbers
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

            return View(barbers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            BarberDetailsViewModel? model = await dbContext.Barbers
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

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
