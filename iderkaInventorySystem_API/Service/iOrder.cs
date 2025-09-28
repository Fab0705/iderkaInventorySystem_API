using iderkaInventorySystem_API.Models;
using System.Collections;

namespace iderkaInventorySystem_API.Service
{
    public interface iOrder
    {
        Task<IEnumerable> GetAllOrders();
        Task<IEnumerable> GetAllDetailOrders();
        Task<object?> GetOrderById(string id);
        Task<object?> GetDetailedOrderByLoc(string idLoc);
        Task Add(Order ordDTO);
        Task<bool> UpdateStatus(string id);
        string GetOrderId();
    }
}
