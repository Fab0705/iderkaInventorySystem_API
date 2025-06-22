using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace iderkaInventorySystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly iOrder _ord;
        public OrdersController(iOrder ord)
        {
            _ord = ord;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _ord.GetAllOrders());

        [HttpGet("by-location/{idLoc}")]
        public async Task<IActionResult> GetByLocation(string idLoc)
        {
            var order = await _ord.GetDetailedOrderByLoc(idLoc);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _ord.GetOrderById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            var ord = new Order
            {
                WorkOrd = json.GetProperty("workOrd").GetString()!,
                DescOrd = json.GetProperty("descOrd").GetString()!,
                StatusOrd = json.GetProperty("statusOrd").GetString()!,
                IdLoc = json.GetProperty("idLoc").GetString()!,
                DetailOrders = json.GetProperty("detailOrders")
                .EnumerateArray()
                .Select(d => new DetailOrder
                {
                    IdSpare = d.GetProperty("idSpare").GetString()!,
                    Quantity = d.GetProperty("quantity").GetInt32()
                }).ToList()
            };
            await _ord.Add(ord);
            return Ok(new {message = "Orden registrada"});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(string id)
        {
            await _ord.UpdateStatus(id);
            return Ok(new { message = "Orden actualizada" });
        }
    }
}
