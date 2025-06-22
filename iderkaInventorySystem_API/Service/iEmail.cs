using iderkaInventorySystem_API.Models;

namespace iderkaInventorySystem_API.Service
{
    public interface iEmail
    {
        Task SendConditionReport(ConditionReport report);
    }
}
