using iderkaInventorySystem_API.Models;

namespace iderkaInventorySystem_API.Service
{
    public interface iTransfer
    {
        Task<IEnumerable<object>> GetAllTransfers();
        Task Add(Transfer transfDTO);
        Task<bool> UpdateStatus(string id);
        Task<object?> GetDetailTransferById(string id);
        Task<object?> GetDetailedTransferByLoc(string idLoc);
        string GetTransferId();
    }
}
