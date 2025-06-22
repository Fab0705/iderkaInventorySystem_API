namespace iderkaInventorySystem_API.Service
{
    public interface iNotification
    {
        Task<object> GetNotificationsForLocation(string idLoc);
    }
}
