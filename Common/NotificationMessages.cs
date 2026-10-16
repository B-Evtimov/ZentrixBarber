using static ZentrixBarber.Common.NotificationMessages;
namespace ZentrixBarber.Common
{
    public static class NotificationMessages
    {
        public const string AppointmentCreated = "Your appointment was booked successfully.";
        public const string AppointmentUpdated = "The appointment was updated successfully.";
        public const string AppointmentNotEditable = "Only pending or confirmed appointments can be edited.";
    }
}