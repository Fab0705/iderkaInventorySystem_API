using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace iderkaInventorySystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SparePartController : Controller
    {
        private readonly iSparePart _spr;
        public SparePartController(iSparePart spr)
        {
            _spr = spr;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _spr.GetAllProducts());

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _spr.GetProdDetailedById(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("by-location/{idLoc}")]
        public async Task<IActionResult> GetByLocation(string idLoc)
        {
            var product = await _spr.GetProdDetailedByLoc(idLoc);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            var sparePart = new SparePart
            {
                NumberPart = json.GetProperty("numberPart").GetString()!,
                DescPart = json.GetProperty("descPart").GetString()!,
                Rework = json.GetProperty("rework").GetBoolean(),
                SparePartStocks = json.GetProperty("sparePartStocks")
            .EnumerateArray()
            .Select(s => new SparePartStock
            {
                IdLoc = s.GetProperty("idLoc").GetString()!,
                Quantity = s.GetProperty("quantity").GetInt32()
            }).ToList()
            };

            await _spr.Add(sparePart);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(SparePart spr)
        {
            await _spr.Update(spr);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _spr.GetProdDetailedById(id);
            if (product == null) return NotFound();

            await _spr.Delete(id);
            return NoContent();
        }
    }
}
