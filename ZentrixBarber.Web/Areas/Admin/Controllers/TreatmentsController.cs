using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.Web.ViewModels.Treatments;
using static ZentrixBarber.Common.NotificationMessages;
using static ZentrixBarber.Common.ValidationMessages;

namespace ZentrixBarber.Web.Areas.Admin.Controllers
{
    public class TreatmentsController : BaseAdminController
    {
        private readonly ITreatmentService treatmentService;

        public TreatmentsController(ITreatmentService treatmentService)
        {
            this.treatmentService = treatmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AdminTreatmentIndexViewModel> treatments = await treatmentService.GetAllTreatmentsAsync();

            return View(treatments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TreatmentFormViewModel model = new TreatmentFormViewModel();

            await treatmentService.PopulateCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TreatmentFormViewModel model)
        {
            await ValidateCategoryAsync(model);

            if (!ModelState.IsValid)
            {
                await treatmentService.PopulateCategoriesAsync(model);
                return View(model);
            }

            await treatmentService.CreateTreatmentAsync(model);

            TempData["SuccessMessage"] = TreatmentCreated;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TreatmentFormViewModel? model = await treatmentService.GetTreatmentForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TreatmentFormViewModel model)
        {
            await ValidateCategoryAsync(model);

            if (!ModelState.IsValid)
            {
                await treatmentService.PopulateCategoriesAsync(model);
                return View(model);
            }

            bool updated = await treatmentService.UpdateTreatmentAsync(id, model);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = TreatmentUpdated;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AdminTreatmentIndexViewModel? model = await treatmentService.GetTreatmentSummaryAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await treatmentService.HasAppointmentsAsync(id))
            {
                TempData["ErrorMessage"] = TreatmentHasAppointments;
                return RedirectToAction(nameof(Index));
            }

            bool deleted = await treatmentService.DeleteTreatmentAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = TreatmentDeleted;

            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateCategoryAsync(TreatmentFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            if (!await treatmentService.CategoryExistsAsync(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryNotFoundMessage);
            }
        }
    }
}