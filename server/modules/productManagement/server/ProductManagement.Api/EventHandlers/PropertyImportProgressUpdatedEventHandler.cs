using Core.Events;
using ProductManagement.Api.Hubs;
using ProductManagement.Core.Services.ImportProperties;
using ProductManagement.Core.SignalR;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;

namespace ProductManagement.Api.EventHandlers
{
    public class PropertyImportProgressUpdatedEventHandler : IConsumer<PropertyImportProgressUpdated>
    {
        public int Order => 100;

        public void HandleEvent(PropertyImportProgressUpdated eventMessage)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<PropertyImportProgressUpdateHub>();
            try
            {
                var connectionIds = HubConnectionManager.GetConnectionsForUser(eventMessage.UserId.ToString()).ToList();

                if (!connectionIds.Any()) return;

                hub.Clients.Clients(connectionIds).updateImportProductProgress(eventMessage.Success, eventMessage.Failed, eventMessage.Total, eventMessage.LastResult, eventMessage.State);
            }
            catch (UnauthorizedAccessException exception) { }
        }
    }
}