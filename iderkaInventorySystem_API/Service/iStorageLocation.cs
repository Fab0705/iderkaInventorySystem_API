using iderkaInventorySystem_API.Models;

namespace iderkaInventorySystem_API.Service
{
    public interface iStorageLocation
    {
        Task<IEnumerable<StorageLocation>> GetAllStorageLocations();
    }
}
