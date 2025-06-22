using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class TransferRepository : iTransfer
    {
        private readonly DBContext dbContext = new DBContext();

        public async Task Add(Transfer transfDTO)
        {
            try
            {
                transfDTO.IdTransf = GetTransferId();
                transfDTO.DateTransf = DateTime.Now; // Asigna la fecha actual

                int currentMaxindex = 1;

                if (dbContext.DetailTransfers.Any())
                {
                    string lastId = dbContext.DetailTransfers.Max(u => u.IdDetTransf);
                    currentMaxindex = int.Parse(lastId.Substring(1)) + 1; // Incrementa el índice
                }

                foreach (var detail in transfDTO.DetailTransfers)
                {
                    detail.IdDetTransf = $"D{currentMaxindex:D4}";
                    detail.IdTransf = transfDTO.IdTransf; // Asigna el IdTransf a cada DetailTransfer
                    currentMaxindex++;
                }

                dbContext.Transfers.Add(transfDTO);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<IEnumerable<object>> GetAllTransfers()
        {
            return await dbContext.Transfers
                .Select (t => new
                {
                    t.IdTransf,
                    t.DateTransf,
                    t.ArrivalDate,
                    OriginLocation = new
                    {
                        t.Origin.IdLoc,
                        t.Origin.NameSt
                    },
                    DestinyLocation = new
                    {
                        t.Destiny.IdLoc,
                        t.Destiny.NameSt
                    },
                    t.StatusTransf
                })
                .ToListAsync();
        }

        public async Task<object?> GetDetailedTransferByLoc(string idLoc)
        {
            return await dbContext.Transfers
                .Where(t => t.Origin.IdLoc == idLoc)
                .Select(t => new
                {
                    t.IdTransf,
                    t.DateTransf,
                    t.ArrivalDate,
                    OriginLocation = new
                    {
                        t.Origin.IdLoc,
                        t.Origin.NameSt
                    },
                    DestinyLocation = new
                    {
                        t.Destiny.IdLoc,
                        t.Destiny.NameSt
                    },
                    t.StatusTransf,
                    SpareParts = t.DetailTransfers.Select(dt => new
                    {
                        dt.IdDetTransf,
                        dt.IdSpare,
                        dt.Quantity,
                        SparePart = new
                        {
                            dt.IdSpareNavigation.DescPart,
                            dt.IdSpareNavigation.NumberPart
                        }
                    })
                })
                .ToListAsync();
        }

        public async Task<object?> GetDetailTransferById(string id)
        {
            var transfer = await dbContext.Transfers
                        .Include(t => t.Origin).ThenInclude(o => o.IdRegNavigation)
                        .Include(t => t.Destiny).ThenInclude(d => d.IdRegNavigation)
                        .Include(t => t.DetailTransfers).ThenInclude(d => d.IdSpareNavigation)
                        .FirstOrDefaultAsync(t => t.IdTransf == id);

            if (transfer == null) return null;

            return new
            {
                transfer.IdTransf,
                Origin = new
                {
                    transfer.Origin.IdLoc,
                    transfer.Origin.NameSt,
                    Region = transfer.Origin.IdRegNavigation.DescReg
                },
                Destiny = new
                {
                    transfer.Destiny.IdLoc,
                    transfer.Destiny.NameSt,
                    Region = transfer.Destiny.IdRegNavigation.DescReg
                },
                transfer.DateTransf,
                transfer.ArrivalDate,
                transfer.StatusTransf,
                SpareParts = transfer.DetailTransfers.Select(dt => new {
                    dt.IdDetTransf,
                    dt.IdSpare,
                    dt.Quantity,
                    dt.IdSpareNavigation.DescPart,
                    dt.IdSpareNavigation.NumberPart
                })
            };
        }

        public string GetTransferId()
        {
            int nextId = 1;

            // hay registros?
            if (dbContext.Transfers.Any())
            {
                string lastId = dbContext.Transfers.Max(u => u.IdTransf);

                int numericPart = int.Parse(lastId.Substring(1));

                nextId = numericPart + 1;
            }

            return $"T{nextId:D4}";
        }

        public async Task<bool> UpdateStatus(string id)
        {
            try
            {
                var transfer = await dbContext.Transfers
                    .Include (t => t.DetailTransfers)
                    .Include (t => t.Origin)
                    .Include(t => t.Destiny)
                    .FirstOrDefaultAsync(t => t.IdTransf == id);

                if (transfer == null) 
                    return false;

                var origin = transfer.Origin.IdReg;
                var destiny = transfer.Destiny.IdReg;

                if (transfer.StatusTransf == "Pendiente")
                {
                    foreach (var t in transfer.DetailTransfers)
                    {
                        var prodStockOrigin = await dbContext.SparePartStocks
                            .Include(ps => ps.IdLocNavigation)
                            .FirstOrDefaultAsync(ps =>
                                ps.IdSpare == t.IdSpare &&
                                ps.IdLocNavigation.IdReg == origin);

                        if (prodStockOrigin == null || prodStockOrigin.Quantity < t.Quantity)
                            return false;

                        prodStockOrigin.Quantity -= t.Quantity;
                    }

                    transfer.StatusTransf = "En tránsito";
                }
                else if (transfer.StatusTransf == "En tránsito")
                {
                    foreach (var t in transfer.DetailTransfers)
                    {
                        var prodStockDestiny = await dbContext.SparePartStocks
                            .Include(ps => ps.IdLocNavigation)
                            .FirstOrDefaultAsync(ps =>
                                ps.IdSpare == t.IdSpare &&
                                ps.IdLocNavigation.IdReg == destiny);

                        if (prodStockDestiny == null)
                            return false;

                        prodStockDestiny.Quantity += t.Quantity;
                    }
                    transfer.ArrivalDate = DateTime.Now; // Asigna la fecha de entrega
                    transfer.StatusTransf = "Entregado";
                }
                else if (transfer.StatusTransf == "Entregado")
                {
                    transfer.StatusTransf = "Completada";
                }
                else
                {
                    return false;
                }

                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
