using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.Web.ViewModels.Barbers;
using static ZentrixBarber.Common.NotificationMessages;

namespace ZentrixBarber.Web.Areas.Admin.Controllers
{
    public class BarbersController : BaseAdminController
    {
        private readonly IBarberService barberService;

        public BarbersController(IBarberService barberService)
        {
            this.barberService = barberService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AdminBarberIndexViewModel> barbers = await barberService.GetAllBarbersAsync();

            return View(barbers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BarberFormViewModel model = new BarberFormViewModel();

            await barberService.PopulateTreatmentsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BarberFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await barberService.PopulateTreatmentsAsync(model);
                return View(model);
            }

            await barberService.CreateBarberAsync(model);

            TempData["SuccessMessage"] = BarberCreated;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            BarberFormViewModel? model = await barberService.GetBarberForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BarberFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await barberService.PopulateTreatmentsAsync(model);
                return View(model);
            }

            bool updated = await barberService.UpdateBarberAsync(id, model);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = BarberUpdated;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            bool? isActive = await barberService.ToggleActiveAsync(id);

            if (isActive == null)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = isActive.Value ? BarberActivated : BarberDeactivated;

            return RedirectToAction(nameof(Index));
        }
    }
}