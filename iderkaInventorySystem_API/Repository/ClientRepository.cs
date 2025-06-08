using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class ClientRepository : iClient
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(Client client)
        {
            try
            {
                client.IdCli = GetClientId();
                dbContext.Clients.Add(client);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Delete(string id)
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.IdCli == id);

            if (client != null)
            {
                dbContext.Clients.Remove(client);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<object>> GetAllClients()
        {
            return await dbContext.Clients
                .Select(c => new
                {
                    c.IdCli,
                    c.FullName,
                    c.IdReg,
                    c.AddressCli,
                    c.IdRegNavigation.DescReg,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetClientById(string id)
        {
            return await dbContext.Clients.Where(c => c.IdCli == id)
                .Select(c => new
                {
                    c.IdCli,
                    c.FullName,
                    c.IdReg,
                    c.AddressCli,
                    c.IdRegNavigation.DescReg,
                })
                .ToListAsync();
        }

        public string GetClientId()
        {
            int nextId = 1;

            // hay registros?
            if (dbContext.Clients.Any())
            {
                string lastId = dbContext.Clients.Max(u => u.IdCli);

                int numericPart = int.Parse(lastId.Substring(1));

                nextId = numericPart + 1;
            }

            return $"C{nextId:D4}";
        }

        public async Task<IEnumerable<object>> GetClientsByRegion(string id)
        {
            return await dbContext.Clients.Where(c => c.IdReg == id)
                .Select(c => new
                {
                    c.IdCli,
                    c.FullName,
                    c.IdReg,
                    c.AddressCli,
                    c.IdRegNavigation.DescReg,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetClientsWithOrders(string id)
        {
            var client = await dbContext.Clients
                .FirstOrDefaultAsync(c => c.IdCli == id);

            if (client == null)
                return Enumerable.Empty<object>();

            /*return client.Orders.Select(order => new
            {
                order.IdOrd,
                order.DateOrd,
                order.StatusOrd
            });*/
            return null;
        }

        public async Task Update(Client client)
        {
            var existingClient = await dbContext.Clients.FirstOrDefaultAsync(c => c.IdCli == client.IdCli);
            if (existingClient != null)
            {
                existingClient.FullName = client.FullName;
                existingClient.IdReg = client.IdReg;
                existingClient.AddressCli = client.AddressCli;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
