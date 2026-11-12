namespace ZentrixBarber.ViewModels.Barbers
{
    public class BarberIndexViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public int TreatmentsCount { get; set; }

    }
}
