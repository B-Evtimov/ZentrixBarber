namespace ZentrixBarber.Web.ViewModels.Treatments
{
    public class AdminTreatmentIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public decimal Price { get; set; }

        public int DurationMinutes { get; set; }

        public int BarbersCount { get; set; }

        public int AppointmentsCount { get; set; }
    }
}