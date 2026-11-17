using ZentrixBarber.Web.ViewModels.Treatments;

namespace ZentrixBarber.Services.Interfaces
{
    public interface ITreatmentService
    {
        Task<IEnumerable<TreatmentCategoryViewModel>> GetTreatmentsByCategoryAsync();
    }
}
