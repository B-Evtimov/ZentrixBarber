using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data;
using ZentrixBarber.Data.Models;
using ZentrixBarber.Data.Models.Enums;
using ZentrixBarber.Services.Interfaces;
using ZentrixBarber.ViewModels.Appointments;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;
using static ZentrixBarber.Common.ValidationMessages;

namespace ZentrixBarber.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AppointmentIndexViewModel>> GetUpcomingAppointmentsAsync()
        {
            return await dbContext.Appointments
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
        }

        public async Task<AppointmentDetailsViewModel?> GetAppointmentDetailsAsync(int id)
        {
            return await dbContext.Appointments
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
        }

        public async Task<AppointmentFormViewModel?> GetAppointmentForEditAsync(int id)
        {
            AppointmentFormViewModel? model = await dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AppointmentFormViewModel
                {
                    ClientName = a.ClientName,
                    ClientPhone = a.ClientPhone,
                    ClientEmail = a.ClientEmail,
                    StartTime = a.StartTime,
                    BarberId = a.BarberId,
                    TreatmentId = a.TreatmentId,
                    Notes = a.Notes
                })
                .FirstOrDefaultAsync();

            if (model != null)
            {
                await PopulateSelectListsAsync(model);
            }

            return model;
        }

        public async Task<AppointmentStatus?> GetAppointmentStatusAsync(int id)
        {
            return await dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => (AppointmentStatus?)a.Status)
                .FirstOrDefaultAsync();
        }

        public async Task PopulateSelectListsAsync(AppointmentFormViewModel model)
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

        public async Task<(string PropertyName, string ErrorMessage)?> ValidateBookingAsync(AppointmentFormViewModel model, int? excludedAppointmentId = null)
        {
            if (model.StartTime <= DateTime.Now)
            {
                return (nameof(model.StartTime), PastDateMessage);
            }

            int? durationMinutes = await dbContext.Treatments
                .Where(t => t.Id == model.TreatmentId)
                .Select(t => (int?)t.DurationMinutes)
                .FirstOrDefaultAsync();

            if (durationMinutes == null)
            {
                return (nameof(model.TreatmentId), TreatmentNotFoundMessage);
            }

            bool barberOffersTreatment = await dbContext.BarberTreatments
                .AnyAsync(bt => bt.BarberId == model.BarberId
                             && bt.TreatmentId == model.TreatmentId
                             && bt.Barber.IsActive);

            if (!barberOffersTreatment)
            {
                return (nameof(model.TreatmentId), BarberDoesNotOfferTreatmentMessage);
            }

            DateTime endTime = model.StartTime.AddMinutes(durationMinutes.Value);
            DateTime openingTime = model.StartTime.Date.AddHours(OpeningHour);
            DateTime closingTime = model.StartTime.Date.AddHours(ClosingHour);

            if (model.StartTime < openingTime || endTime > closingTime)
            {
                return (nameof(model.StartTime), OutsideWorkingHoursMessage);
            }

            bool isBarberBusy = await dbContext.Appointments
                .AnyAsync(a => a.BarberId == model.BarberId
                            && (excludedAppointmentId == null || a.Id != excludedAppointmentId)
                            && a.Status != AppointmentStatus.Cancelled
                            && a.StartTime < endTime
                            && a.StartTime.AddMinutes(a.Treatment.DurationMinutes) > model.StartTime);

            if (isBarberBusy)
            {
                return (string.Empty, BarberBusyMessage);
            }



            return null;

        }


        public async Task CreateAppointmentAsync(AppointmentFormViewModel model)
        {
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

        }

        public async Task UpdateAppointmentAsync(int id, AppointmentFormViewModel model)
        {
            Appointment appointment = await dbContext.Appointments
                .FirstAsync(a => a.Id == id);

            appointment.ClientName = model.ClientName.Trim();
            appointment.ClientPhone = model.ClientPhone.Trim();
            appointment.ClientEmail = model.ClientEmail?.Trim();
            appointment.StartTime = model.StartTime;
            appointment.BarberId = model.BarberId;
            appointment.TreatmentId = model.TreatmentId;
            appointment.Notes = model.Notes?.Trim();

            await dbContext.SaveChangesAsync();

        }

        public async Task ChangeStatusAsync(int id, AppointmentStatus newStatus)
        {
            Appointment appointment = await dbContext.Appointments
                .FirstAsync(a => a.Id == id);

            appointment.Status = newStatus;

            await dbContext.SaveChangesAsync();

        }


        public async Task DeleteAppointmentAsync(int id)
        {
            Appointment appointment = await dbContext.Appointments
                .FirstAsync(a => a.Id == id);

            dbContext.Appointments.Remove(appointment);

            await dbContext.SaveChangesAsync();
        }

        public bool CanBeEdited(AppointmentStatus status)
        {
            return status == AppointmentStatus.Pending || status == AppointmentStatus.Confirmed;
        }

        public bool IsValidStatusTransition(AppointmentStatus currentStatus, AppointmentStatus newStatus)

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
