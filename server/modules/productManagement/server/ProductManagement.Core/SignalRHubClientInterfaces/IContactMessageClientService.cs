using ProductManagement.Core.Dto;

namespace ProductManagement.Core.SignalRHubClientInterfaces
{
    public interface IContactMessageClientService
    {
        void OnMessageReceived(PropertyContactMessageDto message);
    }

    public interface IPropertyViewClientService
    {
        
    }
}