using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class StorageLocationRepository : iStorageLocation
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task<IEnumerable<StorageLocation>> GetAllStorageLocations() => await dbContext.StorageLocations.ToListAsync();
    }
}
