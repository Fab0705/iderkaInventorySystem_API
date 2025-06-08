using iderkaInventorySystem_API.Models;

namespace iderkaInventorySystem_API.Service
{
    public interface iRegion
    {
        Task<IEnumerable<Region>> GetAllRegions();
    }
}
