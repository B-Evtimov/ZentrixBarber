using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.ViewModels.Appointments;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;
using ZentrixBarber.Data.Models;
using ZentrixBarber.Data.Models.Enums;
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

            TempData["SuccessMessage"] = "Your appointment was booked successfully.";

            return RedirectToAction(nameof(Index));
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

        private async Task ValidateBookingRulesAsync(AppointmentFormViewModel model)
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
                            && a.Status != AppointmentStatus.Cancelled
                            && a.StartTime < endTime
                            && a.StartTime.AddMinutes(a.Treatment.DurationMinutes) > model.StartTime);

            if (isBarberBusy)
            {
                ModelState.AddModelError(string.Empty, BarberBusyMessage);
            }
        }
    }
}