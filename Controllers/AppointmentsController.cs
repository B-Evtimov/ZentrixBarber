using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.Data.Models;
using ZentrixBarber.Data.Models.Enums;
using ZentrixBarber.ViewModels.Appointments;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;
using static ZentrixBarber.Common.NotificationMessages;
using static ZentrixBarber.Common.ValidationMessages;

namespace ZentrixBarber.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AppointmentIndexViewModel> appointments = await dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.StartTime >= DateTime.Today)
                .OrderBy(a => a.StartTime)
                .Select(a => new AppointmentIndexViewModel
                {
                    Id = a.Id,
                    ClientName = a.ClientName,
                    ClientPhone = a.ClientPhone,
                    BarberName = a.Barber.FullName,
                    TreatmentName = a.Treatment.Name,
                    StartTime = a.StartTime,
                    DurationMinutes = a.Treatment.DurationMinutes,
                    Price = a.Treatment.Price,
                    Status = a.Status
                })
                .ToListAsync();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            AppointmentDetailsViewModel? model = await dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AppointmentDetailsViewModel
                {
                    Id = a.Id,
                    ClientName = a.ClientName,
                    ClientPhone = a.ClientPhone,
                    ClientEmail = a.ClientEmail,
                    Notes = a.Notes,
                    BarberName = a.Barber.FullName,
                    TreatmentName = a.Treatment.Name,
                    StartTime = a.StartTime,
                    DurationMinutes = a.Treatment.DurationMinutes,
                    Price = a.Treatment.Price,
                    Status = a.Status,
                    CreatedOn = a.CreatedOn
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? barberId)
        {
            AppointmentFormViewModel model = new AppointmentFormViewModel
            {
                BarberId = barberId ?? 0,
                StartTime = DateTime.Today.AddDays(1).AddHours(OpeningHour)
            };

            await LoadSelectListsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                await ValidateBookingRulesAsync(model);
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync(model);
                return View(model);
            }

            Appointment appointment = new Appointment
            {
                ClientName = model.ClientName.Trim(),
                ClientPhone = model.ClientPhone.Trim(),
                ClientEmail = model.ClientEmail?.Trim(),
                StartTime = model.StartTime,
                Notes = model.Notes?.Trim(),
                BarberId = model.BarberId,
                TreatmentId = model.TreatmentId,
                Status = AppointmentStatus.Pending,
                CreatedOn = DateTime.UtcNow
            };

            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = AppointmentCreated;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Appointment? appointment = await dbContext.Appointments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (!CanBeEdited(appointment.Status))
            {
                TempData["ErrorMessage"] = AppointmentNotEditable;
                return RedirectToAction(nameof(Details), new { id });
            }

            AppointmentFormViewModel model = new AppointmentFormViewModel
            {
                ClientName = appointment.ClientName,
                ClientPhone = appointment.ClientPhone,
                ClientEmail = appointment.ClientEmail,
                StartTime = appointment.StartTime,
                BarberId = appointment.BarberId,
                TreatmentId = appointment.TreatmentId,
                Notes = appointment.Notes
            };

            await LoadSelectListsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentFormViewModel model)
        {
            Appointment? appointment = await dbContext.Appointments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (!CanBeEdited(appointment.Status))
            {
                TempData["ErrorMessage"] = AppointmentNotEditable;
                return RedirectToAction(nameof(Details), new { id });
            }

            if (ModelState.IsValid)
            {
                await ValidateBookingRulesAsync(model, id);
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync(model);
                return View(model);
            }

            appointment.ClientName = model.ClientName.Trim();
            appointment.ClientPhone = model.ClientPhone.Trim();
            appointment.ClientEmail = model.ClientEmail?.Trim();
            appointment.StartTime = model.StartTime;
            appointment.BarberId = model.BarberId;
            appointment.TreatmentId = model.TreatmentId;
            appointment.Notes = model.Notes?.Trim();

            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = AppointmentUpdated;

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, AppointmentStatus newStatus)
        {
            Appointment? appointment = await dbContext.Appointments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (!IsValidStatusTransition(appointment.Status, newStatus))
            {
                TempData["ErrorMessage"] = InvalidStatusChange;
                return RedirectToAction(nameof(Details), new { id });
            }

            appointment.Status = newStatus;
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = AppointmentStatusUpdated;

            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task LoadSelectListsAsync(AppointmentFormViewModel model)
        {
            model.Barbers = await dbContext.Barbers
                .AsNoTracking()
                .Where(b => b.IsActive)
                .OrderBy(b => b.FullName)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.FullName
                })
                .ToListAsync();

            model.Treatments = await dbContext.Treatments
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.Name} ({t.DurationMinutes} min, {t.Price:F2} €)"
                })
                .ToListAsync();
        }

        private async Task ValidateBookingRulesAsync(AppointmentFormViewModel model, int? excludedAppointmentId = null)
        {
            if (model.StartTime <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.StartTime), PastDateMessage);
                return;
            }

            int? durationMinutes = await dbContext.Treatments
                .Where(t => t.Id == model.TreatmentId)
                .Select(t => (int?)t.DurationMinutes)
                .FirstOrDefaultAsync();

            if (durationMinutes == null)
            {
                ModelState.AddModelError(nameof(model.TreatmentId), TreatmentNotFoundMessage);
                return;
            }

            bool barberOffersTreatment = await dbContext.BarberTreatments
                .AnyAsync(bt => bt.BarberId == model.BarberId
                             && bt.TreatmentId == model.TreatmentId
                             && bt.Barber.IsActive);

            if (!barberOffersTreatment)
            {
                ModelState.AddModelError(nameof(model.TreatmentId), BarberDoesNotOfferTreatmentMessage);
                return;
            }

            DateTime endTime = model.StartTime.AddMinutes(durationMinutes.Value);
            DateTime openingTime = model.StartTime.Date.AddHours(OpeningHour);
            DateTime closingTime = model.StartTime.Date.AddHours(ClosingHour);

            if (model.StartTime < openingTime || endTime > closingTime)
            {
                ModelState.AddModelError(nameof(model.StartTime), OutsideWorkingHoursMessage);
                return;
            }

            bool isBarberBusy = await dbContext.Appointments
                .AnyAsync(a => a.BarberId == model.BarberId
                            && (excludedAppointmentId == null || a.Id != excludedAppointmentId)
                            && a.Status != AppointmentStatus.Cancelled
                            && a.StartTime < endTime
                            && a.StartTime.AddMinutes(a.Treatment.DurationMinutes) > model.StartTime);

            if (isBarberBusy)
            {
                ModelState.AddModelError(string.Empty, BarberBusyMessage);
            }
        }

        private static bool CanBeEdited(AppointmentStatus status)
        {
            return status == AppointmentStatus.Pending || status == AppointmentStatus.Confirmed;
        }

        private static bool IsValidStatusTransition(AppointmentStatus currentStatus, AppointmentStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (AppointmentStatus.Pending, AppointmentStatus.Confirmed) => true,
                (AppointmentStatus.Pending, AppointmentStatus.Cancelled) => true,
                (AppointmentStatus.Confirmed, AppointmentStatus.Completed) => true,
                (AppointmentStatus.Confirmed, AppointmentStatus.Cancelled) => true,
                _ => false
            };
        }
    }
}