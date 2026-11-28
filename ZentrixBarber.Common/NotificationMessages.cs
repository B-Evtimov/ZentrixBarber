using static ZentrixBarber.Common.NotificationMessages;
namespace ZentrixBarber.Common
{
    public static class NotificationMessages
    {
        public const string AppointmentCreated = "Your appointment was booked successfully.";
        public const string AppointmentUpdated = "The appointment was updated successfully.";
        public const string AppointmentNotEditable = "Only pending or confirmed appointments can be edited.";
        public const string AppointmentStatusUpdated = "The appointment status was updated successfully.";
        public const string InvalidStatusChange = "This status change is not allowed.";
        public const string AppointmentDeleted = "The appointment was deleted successfully.";
        public const string AppointmentCancelled = "Your appointment was cancelled.";
        public const string AppointmentCannotBeCancelled = "This appointment can no longer be cancelled.";
        public const string BarberCreated = "The barber was added successfully.";
        public const string BarberUpdated = "The barber was updated successfully.";
        public const string BarberActivated = "The barber is now active and can be booked.";
        public const string BarberDeactivated = "The barber was deactivated and is hidden from clients.";
        public const string TreatmentCreated = "The service was added successfully.";
        public const string TreatmentUpdated = "The service was updated successfully.";
        public const string TreatmentDeleted = "The service was deleted successfully.";
        public const string TreatmentHasAppointments = "This service has booked appointments and cannot be deleted.";

    }
}