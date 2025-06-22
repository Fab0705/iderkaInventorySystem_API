using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class NotificationsRepository : iNotification
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task<object> GetNotificationsForLocation(string idLoc)
        {
            var lowStockParts = await dbContext.SparePartStocks
            .Where(s => s.IdLoc == idLoc && s.Quantity < 5)
            .Select(s => new
            {
                s.IdSpareNavigation.NumberPart,
                s.IdSpareNavigation.DescPart,
                s.Quantity,
                Level = s.Quantity < 1 ? "Crítico" : "Bajo"
            })
            .ToListAsync();

            var incomingTransfers = await dbContext.Transfers
            .Where(t => t.DestinyId == idLoc && t.StatusTransf == "En tránsito")
            .Select(t => new
            {
                t.IdTransf,
                Fecha = t.DateTransf,
                Origen = t.Origin.NameSt
            })
            .ToListAsync();

            return new
            {
                LowStock = lowStockParts,
                IncomingTransfers = incomingTransfers
            };
        }
    }
}
