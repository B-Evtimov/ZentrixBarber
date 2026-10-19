using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.ViewModels.Treatments;

namespace ZentrixBarber.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly ITreatmentService treatmentService;

        public TreatmentsController(ITreatmentService treatmentService)
        {
            this.treatmentService = treatmentService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<TreatmentCategoryViewModel> categories = await treatmentService.GetTreatmentsByCategoryAsync();

            return View(categories);
        }


    }
}
