using ZentrixBarber.Data.Models.Enums;
using ZentrixBarber.ViewModels.Appointments;

namespace ZentrixBarber.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentIndexViewModel>> GetUpcomingAppointmentsAsync();

        Task<AppointmentDetailsViewModel?> GetAppointmentDetailsAsync(int id);

        Task<AppointmentFormViewModel?> GetAppointmentForEditAsync(int id);

        Task<AppointmentStatus?> GetAppointmentStatusAsync(int id);

        Task PopulateSelectListsAsync(AppointmentFormViewModel model);

        Task<(string PropertyName, string ErrorMessage)?> ValidateBookingAsync(AppointmentFormViewModel model, int? excludedAppointmentId = null);

        Task CreateAppointmentAsync(AppointmentFormViewModel model);

        Task UpdateAppointmentAsync(int id, AppointmentFormViewModel model);

        Task ChangeStatusAsync(int id, AppointmentStatus newStatus);

        Task DeleteAppointmentAsync(int id);

        bool CanBeEdited(AppointmentStatus status);

        bool IsValidStatusTransition(AppointmentStatus currentStatus, AppointmentStatus newStatus);
    }
}
