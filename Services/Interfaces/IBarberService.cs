using ZentrixBarber.ViewModels.Barbers;

namespace ZentrixBarber.Services.Interfaces
{
    public interface IBarberService
    {
        Task<IEnumerable<BarberIndexViewModel>> GetActiveBarbersAsync();

        Task<BarberDetailsViewModel?> GetBarberDetailsAsync(int id);
    }
}
