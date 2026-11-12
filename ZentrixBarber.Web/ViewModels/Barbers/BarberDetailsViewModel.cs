namespace ZentrixBarber.ViewModels.Barbers
{
    public class BarberDetailsViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public IEnumerable<BarberTreatmentViewModel> Treatments { get; set; }
                    = new List<BarberTreatmentViewModel>();
    }
}
