namespace ZentrixBarber.Web.ViewModels.Barbers
{
    public class AdminBarberIndexViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public int YearsOfExperience { get; set; }

        public int TreatmentsCount { get; set; }

        public int UpcomingAppointmentsCount { get; set; }

        public bool IsActive { get; set; }
    }
}