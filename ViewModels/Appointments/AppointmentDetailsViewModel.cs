using ZentrixBarber.Data.Models.Enums;

namespace ZentrixBarber.ViewModels.Appointments
{
    public class AppointmentDetailsViewModel
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public string ClientPhone { get; set; } = null!;
        public string? ClientEmail { get; set; }
        public string? Notes { get; set; }
        public string BarberName { get; set; } = null!;
        public string TreatmentName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);


    }
}
