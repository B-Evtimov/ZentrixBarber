namespace ZentrixBarber.ViewModels.Treatments
{
    public class TreatmentCategoryViewModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IEnumerable<TreatmentCardViewModel> Treatments { get; set; } = new List<TreatmentCardViewModel>();
    }
}
