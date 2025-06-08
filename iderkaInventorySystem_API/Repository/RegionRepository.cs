using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class RegionRepository : iRegion
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task<IEnumerable<Region>> GetAllRegions() => await dbContext.Regions.ToListAsync();
    }
}
