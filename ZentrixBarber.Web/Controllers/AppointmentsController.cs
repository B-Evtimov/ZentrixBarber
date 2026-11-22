using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.Web.Extensions;
using ZentrixBarber.Web.ViewModels.Appointments;
using static ZentrixBarber.Common.ApplicationConstants;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;
using static ZentrixBarber.Common.NotificationMessages;

namespace ZentrixBarber.Web.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            IEnumerable<AppointmentIndexViewModel> appointments = await appointmentService.GetClientAppointmentsAsync(User.GetUserId()!);

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? barberId)
        {
            AppointmentFormViewModel model = new AppointmentFormViewModel
            {
                BarberId = barberId ?? 0,
                ClientEmail = User.Identity?.Name,
                StartTime = DateTime.Today.AddDays(1).AddHours(OpeningHour)
            };

            await appointmentService.PopulateSelectListsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentFormViewModel model)
        {
            await ValidateBookingAsync(model);

            if (!ModelState.IsValid)
            {
                await appointmentService.PopulateSelectListsAsync(model);
                return View(model);
            }

            await appointmentService.CreateAppointmentAsync(model, User.GetUserId()!);

            TempData["SuccessMessage"] = AppointmentCreated;

            if (User.IsInRole(AdministratorRoleName))
            {
                return RedirectToAction("Index", "Appointments", new { area = "Admin" });
            }

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            bool cancelled = await appointmentService.CancelClientAppointmentAsync(id, User.GetUserId()!);

            if (cancelled)
            {
                TempData["SuccessMessage"] = AppointmentCancelled;
            }
            else
            {
                TempData["ErrorMessage"] = AppointmentCannotBeCancelled;
            }

            return RedirectToAction(nameof(Mine));
        }

        private async Task ValidateBookingAsync(AppointmentFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            var bookingError = await appointmentService.ValidateBookingAsync(model);

            if (bookingError != null)
            {
                ModelState.AddModelError(bookingError.Value.PropertyName, bookingError.Value.ErrorMessage);
            }
        }
    }
}