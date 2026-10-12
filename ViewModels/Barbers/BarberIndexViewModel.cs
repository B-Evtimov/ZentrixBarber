namespace ZentrixBarber.ViewModels.Barbers
{
    public class BarberIndexViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string YearsOfExperience { get; set; }
        public string TreatmentsCount { get; set; }

    }
}
