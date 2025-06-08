using iderkaInventorySystem_API.Models;
using System.Collections;

namespace iderkaInventorySystem_API.Service
{
    public interface iOrder
    {
        Task<IEnumerable> GetAllOrders();
        Task<object?> GetOrderById(string id);
        Task<object?> GetOrderByStatus(string status);
        Task Add(Order ordDTO);
        Task<bool> UpdateStatus(string id);
        string GetOrderId();
        string GetDetOrderId(int nextId);
    }
}
