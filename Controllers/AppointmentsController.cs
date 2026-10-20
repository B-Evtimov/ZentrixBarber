using Microsoft.AspNetCore.Mvc;
using ZentrixBarber.Data.Models.Enums;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.ViewModels.Appointments;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;
using static ZentrixBarber.Common.NotificationMessages;
using Microsoft.AspNetCore.Authorization;
using static ZentrixBarber.Common.ApplicationConstants;

namespace ZentrixBarber.Controllers
{
    [Authorize(Roles = AdminRoleName)]
    public class AppointmentsController : Controller
    {

        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;


        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AppointmentIndexViewModel> appointments = await appointmentService.GetUpcomingAppointmentsAsync();

            return View(appointments);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            AppointmentDetailsViewModel? model = await appointmentService.GetAppointmentDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Create(int? barberId)
        {
            AppointmentFormViewModel model = new AppointmentFormViewModel
            {
                BarberId = barberId ?? 0,
                StartTime = DateTime.Today.AddDays(1).AddHours(OpeningHour)
            };

            await appointmentService.PopulateSelectListsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(AppointmentFormViewModel model)
        {
            await ValidateBookingAsync(model);

            if (!ModelState.IsValid)
            {
                await appointmentService.PopulateSelectListsAsync(model);
                return View(model);
            }

            await appointmentService.CreateAppointmentAsync(model);

            TempData["SuccessMessage"] = AppointmentCreated;

            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AppointmentStatus? status = await appointmentService.GetAppointmentStatusAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            if (!appointmentService.CanBeEdited(status.Value))
            {
                TempData["ErrorMessage"] = AppointmentNotEditable;
                return RedirectToAction(nameof(Details), new { id });
            }

            AppointmentFormViewModel? model = await appointmentService.GetAppointmentForEditAsync(id);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentFormViewModel model)
        {
            AppointmentStatus? status = await appointmentService.GetAppointmentStatusAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            if (!appointmentService.CanBeEdited(status.Value))
            {
                TempData["ErrorMessage"] = AppointmentNotEditable;
                return RedirectToAction(nameof(Details), new { id });
            }

            await ValidateBookingAsync(model, id);

            if (!ModelState.IsValid)
            {
                await appointmentService.PopulateSelectListsAsync(model);
                return View(model);
            }

            await appointmentService.UpdateAppointmentAsync(id, model);

            TempData["SuccessMessage"] = AppointmentUpdated;

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, AppointmentStatus newStatus)
        {
            AppointmentStatus? currentStatus = await appointmentService.GetAppointmentStatusAsync(id);

            if (currentStatus == null)
            {
                return NotFound();
            }

            if (!appointmentService.IsValidStatusTransition(currentStatus.Value, newStatus))
            {
                TempData["ErrorMessage"] = InvalidStatusChange;
                return RedirectToAction(nameof(Details), new { id });
            }

            await appointmentService.ChangeStatusAsync(id, newStatus);

            TempData["SuccessMessage"] = AppointmentStatusUpdated;

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AppointmentDetailsViewModel? model = await appointmentService.GetAppointmentDetailsAsync(id);

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
            AppointmentStatus? status = await appointmentService.GetAppointmentStatusAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            await appointmentService.DeleteAppointmentAsync(id);

            TempData["SuccessMessage"] = AppointmentDeleted;

            return RedirectToAction(nameof(Index));
        }




        private async Task ValidateBookingAsync(AppointmentFormViewModel model, int? excludedAppointmentId = null)
        {

            if (!ModelState.IsValid)
            {
                return;
            }

            var bookingError = await appointmentService.ValidateBookingAsync(model, excludedAppointmentId);

            if (bookingError != null)
            {
                ModelState.AddModelError(bookingError.Value.PropertyName, bookingError.Value.ErrorMessage);
            }



        }
    }
}
