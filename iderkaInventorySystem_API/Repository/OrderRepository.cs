using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace iderkaInventorySystem_API.Repository
{
    public class OrderRepository : iOrder
    {
        private readonly DBContext dbContext = new DBContext();

        public async Task Add(Order ordDTO)
        {
            try
            {
                ordDTO.IdOrd = GetOrderId();
                ordDTO.DateOrd = DateTime.Now;
                ordDTO.StatusOrd = "Entregado"; // Se registra directamente como entregado

                int currentMaxIndex = 1;

                if (dbContext.DetailOrders.Any())
                {
                    string lastId = dbContext.DetailOrders.Max(u => u.IdDetOrd);
                    currentMaxIndex = int.Parse(lastId.Substring(1)) + 1;
                }

                foreach (var detail in ordDTO.DetailOrders)
                {
                    detail.IdDetOrd = $"D{currentMaxIndex:D4}";
                    detail.IdOrd = ordDTO.IdOrd;
                    currentMaxIndex++;

                    // Buscar el stock del repuesto en la ubicación correspondiente
                    var stock = await dbContext.SparePartStocks
                        .FirstOrDefaultAsync(s =>
                            s.IdSpare == detail.IdSpare &&
                            s.IdLoc == ordDTO.IdLoc); // el IdLoc debe venir desde el cliente

                    if (stock == null)
                        throw new Exception($"No hay stock del repuesto {detail.IdSpare} en la ubicación {ordDTO.IdLoc}");

                    if (stock.Quantity < detail.Quantity)
                        throw new Exception($"Stock insuficiente para el repuesto {detail.IdSpare}");

                    stock.Quantity -= detail.Quantity;
                }

                dbContext.Orders.Add(ordDTO);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar orden:", ex);
                throw; // o manejar el error como prefieras
            }
        }

        public async Task<IEnumerable> GetAllOrders() => await dbContext.Orders.Include(o => o.DetailOrders).ToListAsync();

        public async Task<object?> GetDetailedOrderByLoc(string idLoc)
        {
            return await dbContext.Orders
                .Where(o => o.IdLoc == idLoc)
                .Select(o => new
                {
                    o.IdOrd,
                    o.WorkOrd,
                    o.DescOrd,
                    o.DateOrd,
                    o.StatusOrd,
                    SpareParts = o.DetailOrders.Select(p => new
                    {
                        p.IdDetOrd,
                        p.IdSpare,
                        p.IdSpareNavigation.NumberPart,
                        p.IdSpareNavigation.DescPart,
                        p.Quantity,
                    })
                })
                .ToListAsync();
        }

        public async Task<object?> GetOrderById(string id)
        {
            var order = await dbContext.Orders
                .Include(o => o.DetailOrders).ThenInclude(o => o.IdSpareNavigation)
                .FirstOrDefaultAsync(o => o.IdOrd == id);

            if (order == null) return null;

            return new
            {
                order.IdOrd,
                order.WorkOrd,
                order.DescOrd,
                order.DateOrd,
                order.StatusOrd,
                SpareParts = order.DetailOrders.Select(p => new
                {
                    p.IdDetOrd,
                    p.IdSpare,
                    p.IdSpareNavigation.NumberPart,
                    p.IdSpareNavigation.DescPart,
                    p.Quantity,
                })
            };
        }

        public async Task<IEnumerable> GetAllDetailOrders() => await dbContext.DetailOrders.Include(od => od.IdSpareNavigation).ToListAsync();

        public string GetOrderId()
        {
            int nextId = 1;

            // hay registros?
            if (dbContext.Orders.Any())
            {
                string lastId = dbContext.Orders.Max(u => u.IdOrd);

                int numericPart = int.Parse(lastId.Substring(1));

                nextId = numericPart + 1;
            }

            return $"O{nextId:D4}";
        }

        public async Task<bool> UpdateStatus(string id)
        {
            try
            {
                var order = await dbContext.Orders
                    .Include(o => o.DetailOrders)
                    .FirstOrDefaultAsync(o => o.IdOrd == id);

                if (order == null)
                    return false;

                if (order.StatusOrd == "Pendiente")
                {
                    foreach (var detail in order.DetailOrders)
                    {
                        // Buscar el stock de ese producto en una ubicación que pertenezca a la región del cliente
                        var prodStock = await dbContext.SparePartStocks
                            .Include(ps => ps.IdLocNavigation)
                            .FirstOrDefaultAsync(ps =>
                                ps.IdSpare == detail.IdSpare &&
                                ps.IdLoc == order.IdLoc);

                        if (prodStock == null || prodStock.Quantity < detail.Quantity)
                            return false;

                        prodStock.Quantity -= detail.Quantity;
                    }

                    order.StatusOrd = "Proceso";
                }
                else if (order.StatusOrd == "Proceso")
                {
                    order.StatusOrd = "Entregado";
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
