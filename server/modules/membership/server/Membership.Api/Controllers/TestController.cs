using Core.Testing.Runner;
using Membership.Library.Tests.Integration.AdminLoginTest;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/tests")]
    public class TestController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult RunTests()
        {
            return Ok(new TestRunner().Run(typeof(AdminTests).Assembly)); 
        }
    }
}
