using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Membership.Core;
using Membership.Core.Contracts.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;

namespace ProductManagement.Core.SignalR
{
    public class AuthenticationEnabledGenericHub : AuthenticationEnabledHub<dynamic>
    {
    }

    public class AuthenticationEnabledHub<T> : Hub<T> where T : class
    {
        private IMembership Membership
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMembership>();
            }
        }

        protected readonly ConnectionMapping<string> Connections = HubConnectionManager.Connections;

        protected IEnumerable<string> GetConnectionsForUser(string userId)
        {
            return Connections.GetConnections(userId);
        }

        protected string GetUserId(IRequest request)
        {
            var userIdInLong = GetUserIdInLong(request);
            return userIdInLong == 0 ? "" : userIdInLong.ToString();
        }

        public long GetUserIdInLong(IRequest request)
        {
            string authToken = null;
            // first check the header
            if (request.Headers.Any(h => h.Key == WebAuthorizeConst.AuthorizeTokenName))
            {
                var authTokenValue = request.Headers.First(h => h.Key == WebAuthorizeConst.AuthorizeTokenName);
                authToken = authTokenValue.Value;
            }

            if (request.QueryString.Any(h => h.Key == WebAuthorizeConst.AuthorizeTokenName))
            {
                var authTokenValue = request.QueryString.First(h => h.Key == WebAuthorizeConst.AuthorizeTokenName);
                authToken = authTokenValue.Value;
            }

            if (string.IsNullOrEmpty(authToken)) return 0;

            var result = Membership.ValidateUser(authToken);
            if (!result.IsSuccess || Membership.GetCurrentUser() == null) return 0;

            return Membership.UserId;
        }

        public override Task OnReconnected()
        {
            var userId = GetUserId(Context.Request);

            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        public override Task OnConnected()
        {
            var userId = GetUserId(Context.Request);
            Connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = GetUserId(Context.Request);
            Connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}