using iderkaInventorySystem_API.Models;
using System.Collections;

namespace iderkaInventorySystem_API.Service
{
    public interface iSparePart
    {
        Task<IEnumerable<object>> GetAllProducts();
        Task<object?> GetProdDetailedById (string id);
        Task<object?> GetProdDetailedByNumPart(string numPart, string locId);
        Task<object?> GetProdDetailedByLoc(string idLoc);
        Task<bool> UpdateStock(string idSpare, string idLoc, int quantity);
        Task Add(SparePart spr);
        Task Update(SparePart updatedProduct);
        Task Delete(string id);
        string GetSparePartId();
    }
}
