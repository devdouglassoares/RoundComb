using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace Core.WebApi.Cors
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        readonly CorsPolicy _policy;
        public CorsPolicyProvider(CorsPolicy policy)
        {
            _policy = policy;
        }
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }

}
