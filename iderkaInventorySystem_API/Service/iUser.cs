using iderkaInventorySystem_API.Models;

namespace iderkaInventorySystem_API.Service
{
    public interface iUser
    {
        Task<IEnumerable<object>> GetAllUsers();
        Task<object> Login(string username, string password);
        Task<object?> GetUserById(string id);
        Task Add(User usr, List<string> roleIds);
        Task Update(User usr);
        Task Delete(string id);
        string GetUserId();
    }
}
