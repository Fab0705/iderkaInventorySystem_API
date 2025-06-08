using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class SparePartRepository : iSparePart
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(SparePart spr)
        {
            try
            {
                spr.IdSpare = GetSparePartId();

                foreach (var stock in spr.SparePartStocks)
                {
                    stock.IdSpare = spr.IdSpare; // Asigna el IdSpare a cada SparePartStock
                }
                dbContext.SpareParts.Add(spr);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Delete(string id)
        {
            var product = await dbContext.SpareParts
                        .Include(p => p.SparePartStocks)
                        .FirstOrDefaultAsync(p => p.IdSpare == id);

            if (product != null)
            {
                dbContext.SparePartStocks.RemoveRange(product.SparePartStocks);
                
                dbContext.SpareParts.Remove(product);

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<object>> GetAllProducts()
        {
            return await dbContext.SpareParts
            .Select(sp => new
            {
                sp.IdSpare,
                sp.NumberPart,
                sp.DescPart,
                sp.Rework,
                SparePartStocks = sp.SparePartStocks.Select(ps => new
                {
                    ps.IdLoc,
                    ps.Quantity,
                    Location = new
                    {
                        ps.IdLocNavigation.IdLoc,
                        ps.IdLocNavigation.NameSt,
                        ps.IdLocNavigation.DescStLoc
                    }
                }).ToList()
            })
            .ToListAsync();
        }

        public async Task<object?> GetProdDetailedById(string id)
        {
            return await dbContext.SpareParts
                .Where(sp => sp.IdSpare == id)
                .Select(sp => new
                {
                    sp.IdSpare,
                    sp.NumberPart,
                    sp.DescPart,
                    sp.Rework,
                    SparePartStocks = sp.SparePartStocks.Select(ps => new
                    {
                        ps.IdLoc,
                        ps.Quantity,
                        Location = new
                        {
                            ps.IdLocNavigation.IdLoc,
                            ps.IdLocNavigation.NameSt,
                            ps.IdLocNavigation.DescStLoc
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<object?> GetProdDetailedByLoc(string idLoc)
        {
            return await dbContext.SpareParts
                .Where(sp => sp.SparePartStocks.Any(ps => ps.IdLoc == idLoc))
                .Select(sp => new
                {
                    sp.IdSpare,
                    sp.NumberPart,
                    sp.DescPart,
                    sp.Rework,
                    SparePartStocks = sp.SparePartStocks
                        .Where(ps => ps.IdLoc == idLoc)
                        .Select(ps => new
                        {
                            ps.IdLoc,
                            ps.Quantity,
                            Location = new
                            {
                                ps.IdLocNavigation.IdLoc,
                                ps.IdLocNavigation.NameSt,
                                ps.IdLocNavigation.DescStLoc
                            }
                        }).ToList()
                })
                .ToListAsync();
        }

        public string GetSparePartId()
        {
            int nextId = 1;

            // hay registros?
            if (dbContext.SpareParts.Any())
            {
                string lastId = dbContext.SpareParts.Max(u => u.IdSpare);

                int numericPart = int.Parse(lastId.Substring(1));

                nextId = numericPart + 1;
            }

            return $"P{nextId:D4}";
        }

        public async Task Update(SparePart updatedProduct)
        {
            var existingProduct = await dbContext.SpareParts
                                .Include(p => p.SparePartStocks)
                                .FirstOrDefaultAsync(p => p.IdSpare == updatedProduct.IdSpare);

            if (existingProduct != null)
            {
                // Actualiza campos simples
                existingProduct.NumberPart = updatedProduct.NumberPart;
                existingProduct.DescPart = updatedProduct.DescPart;
                existingProduct.Rework = updatedProduct.Rework;

                // Limpia el stock existente si vas a reemplazar completamente
                dbContext.SparePartStocks.RemoveRange(existingProduct.SparePartStocks);

                // Agrega el nuevo stock
                existingProduct.SparePartStocks = updatedProduct.SparePartStocks;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
