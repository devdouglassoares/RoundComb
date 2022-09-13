using System.Web.Http;
using Core.Testing.Runner;
using Membership.Library.Tests.Integration.AdminLoginTest;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/tests")]
    public class TestsController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult RunTests()
        {
            return Ok(new TestRunner().Run(typeof(AdminTests).Assembly));
        }
    }
}
