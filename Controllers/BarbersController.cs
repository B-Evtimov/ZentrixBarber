using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.ViewModels.Barbers;

namespace ZentrixBarber.Controllers
{
    public class BarbersController : Controller
    {
        private readonly IBarberService barberService;

        public BarbersController(IBarberService barberService)
        {
            this.barberService = barberService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BarberIndexViewModel> barbers = await barberService.GetActiveBarbersAsync();

            return View(barbers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            BarberDetailsViewModel? model = await barberService.GetBarberDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }


            return View(model);


        }
    }


}
