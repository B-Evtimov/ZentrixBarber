using ZentrixBarber.Web.ViewModels.Treatments;

namespace ZentrixBarber.Services.Interfaces
{
    public interface ITreatmentService
    {
        Task<IEnumerable<TreatmentCategoryViewModel>> GetTreatmentsByCategoryAsync();

        Task<IEnumerable<AdminTreatmentIndexViewModel>> GetAllTreatmentsAsync();

        Task<AdminTreatmentIndexViewModel?> GetTreatmentSummaryAsync(int id);

        Task<TreatmentFormViewModel?> GetTreatmentForEditAsync(int id);

        Task PopulateCategoriesAsync(TreatmentFormViewModel model);

        Task<bool> CategoryExistsAsync(int categoryId);

        Task CreateTreatmentAsync(TreatmentFormViewModel model);

        Task<bool> UpdateTreatmentAsync(int id, TreatmentFormViewModel model);

        Task<bool> HasAppointmentsAsync(int id);

        Task<bool> DeleteTreatmentAsync(int id);
    }
}