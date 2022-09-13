using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace ProductManagement.Core.SignalR
{
    public class HubConnectionManager
    {
        public static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        public static IEnumerable<string> GetConnectionsForUser(string userId)
        {
            return Connections.GetConnections(userId);
        }
    }

    public static class HubExtensions
    {
        public static bool BroadcastToClient<T>(this AuthenticationEnabledHub<T> hubInstance, string userId, Action<T> callback) where T : class
        {
            var sent = false;
            foreach (var connectionId in HubConnectionManager.GetConnectionsForUser(userId))
            {
                callback(hubInstance.Clients.Client(connectionId));
                sent = true;
            }
            return sent;
        }
        public static void BroadcastToAllClient<T>(this AuthenticationEnabledHub<T> hubInstance, Action<T> callback) where T : class
        {
            callback(hubInstance.Clients.All);
        }

        public static bool BroadcastToClient(this IHubContext hubInstance, string userId, Action<dynamic> callback)
        {
            var sent = false;
            foreach (var connectionId in HubConnectionManager.GetConnectionsForUser(userId))
            {
                callback(hubInstance.Clients.Client(connectionId));
                sent = true;
            }
            return sent;
        }

        public static void BroadcastToAllClients(this IHubContext hubInstance, Action<dynamic> callback)
        {
            callback(hubInstance.Clients.All);
        }
    }
}