using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace iderkaInventorySystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : Controller
    {
        private readonly iTransfer _tran;
        private readonly iEmail _emailService;
        public TransferController(iTransfer tran, iEmail emailService)
        {
            _tran = tran;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransfers() => Ok(await _tran.GetAllTransfers());

        [HttpGet("by-origin/{idLoc}")]
        public async Task<IActionResult> GetTransferByLocation(string idLoc)
        {
            var transfer = await _tran.GetDetailedTransferByLoc(idLoc);
            return transfer == null ? NotFound() : Ok(transfer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransferId(string id)
        {
            var transfer = await _tran.GetDetailTransferById(id);
            return transfer == null ? NotFound() : Ok(transfer);
        }

        [HttpPost("email/sendReport")]
        public async Task<IActionResult> SendConditionReport([FromBody] ConditionReport report)
        {
            await _emailService.SendConditionReport(report);
            return Ok(new { message = "Correo enviado correctamente." });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            var transf = new Transfer
            {
                OriginId = json.GetProperty("originId").GetString()!,
                DestinyId = json.GetProperty("destinyId").GetString()!,
                StatusTransf = json.GetProperty("statusTransf").GetString()!,
                DetailTransfers = json.GetProperty("detailTransfers")
                .EnumerateArray()
                .Select(d => new DetailTransfer
                {
                    IdSpare = d.GetProperty("idSpare").GetString()!,
                    Quantity = d.GetProperty("quantity").GetInt32()
                }).ToList()
            };
            await _tran.Add(transf);
            return Ok(new { message = "Transferencia registrada" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus (string id)
        {
            await _tran.UpdateStatus(id);
            return Ok(new { message = "Estado de la transferencia actualizada" });
        }
    }
}
