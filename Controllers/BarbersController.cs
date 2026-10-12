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
    }
}
