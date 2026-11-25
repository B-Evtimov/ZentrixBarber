using ZentrixBarber.Web.ViewModels.Barbers;

namespace ZentrixBarber.Services.Interfaces
{
    public interface IBarberService
    {
        Task<IEnumerable<BarberIndexViewModel>> GetActiveBarbersAsync();

        Task<BarberDetailsViewModel?> GetBarberDetailsAsync(int id);

        Task<IEnumerable<AdminBarberIndexViewModel>> GetAllBarbersAsync();

        Task<BarberFormViewModel?> GetBarberForEditAsync(int id);

        Task PopulateTreatmentsAsync(BarberFormViewModel model);

        Task<int> CreateBarberAsync(BarberFormViewModel model);

        Task<bool> UpdateBarberAsync(int id, BarberFormViewModel model);

        Task<bool?> ToggleActiveAsync(int id);

    }
}
