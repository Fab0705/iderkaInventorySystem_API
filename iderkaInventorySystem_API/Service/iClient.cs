using iderkaInventorySystem_API.Models;

namespace iderkaInventorySystem_API.Service
{
    public interface iClient
    {
        Task<IEnumerable<object>> GetAllClients();
        Task<IEnumerable<object>> GetClientById(string id);
        Task<IEnumerable<object>> GetClientsByRegion(string id);
        Task<IEnumerable<object>> GetClientsWithOrders(string id);
        Task Add(Client client);
        Task Update(Client client);
        Task Delete(string id);
        string GetClientId();
    }
}
