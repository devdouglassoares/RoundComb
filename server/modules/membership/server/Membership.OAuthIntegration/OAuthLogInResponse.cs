using System.Net;

namespace Membership.OAuthIntegration
{
    public class OAuthLogInResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public OAuthUser UserInfomation { get; set; }
    }
}