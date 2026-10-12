namespace ZentrixBarber.ViewModels.Treatments
{
    public class TreatmentCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
    }
}
