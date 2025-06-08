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
                ordDTO.DateOrd = DateTime.Now; // Asigna la fecha actual

                int index = 1;

                foreach (var detail in ordDTO.DetailOrders)
                {
                    detail.IdDetOrd = GetDetOrderId(index);
                    detail.IdOrd = ordDTO.IdOrd; // Asigna el IdOrd a cada DetailOrder
                    index++;
                }

                dbContext.Orders.Add(ordDTO);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<IEnumerable> GetAllOrders() => await dbContext.Orders.ToListAsync();

        public string GetDetOrderId(int nextId)
        {
            // hay registros?
            if (dbContext.DetailOrders.Any())
            {
                string lastId = dbContext.DetailOrders.Max(u => u.IdDetOrd);

                int numericPart = int.Parse(lastId.Substring(1));

                nextId = numericPart + 1;
            }

            return $"D{nextId:D4}";
            //return $"D{index:D4}";
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
                    p.IdSpareNavigation.DescPart,
                    p.Quantity,
                })
            };
        }

        public Task<object?> GetOrderByStatus(string status)
        {
            throw new NotImplementedException();
        }

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
