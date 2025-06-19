using iderkaInventorySystem_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace iderkaInventorySystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller 
    {
        private readonly iRegion _reg;
        private readonly iStorageLocation _stl;
        public DataController(iRegion reg, iStorageLocation stl)
        {
            _reg = reg;
            _stl = stl;
        }

        [HttpGet("region")]
        public async Task<IActionResult> GetRegions() => Ok(await _reg.GetAllRegions());

        [HttpGet("location")]
        public async Task<IActionResult> GetLocation() => Ok(await _stl.GetAllStorageLocations());
    }
}
