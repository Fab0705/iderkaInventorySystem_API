using iderkaInventorySystem_API.Models;
using System.Collections;

namespace iderkaInventorySystem_API.Service
{
    public interface iSparePart
    {
        Task<IEnumerable<object>> GetAllProducts();
        Task<object?> GetProdDetailedById (string id);
        Task<object?> GetProdDetailedByNumPart(string numPart);
        Task<object?> GetProdDetailedByLoc(string idLoc);
        Task Add(SparePart spr);
        Task Update(SparePart updatedProduct);
        Task Delete(string id);
        string GetSparePartId();
    }
}
