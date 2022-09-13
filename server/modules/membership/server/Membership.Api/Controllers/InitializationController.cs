using System;
using System.Configuration;
using System.Web.Http;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Library;
using Membership.Library.Contracts;
using RestSharp;

namespace Membership.Api.Controllers
{
    public class InitializationController : ApiController
    {
        private readonly IMembership membership;
        private readonly IUserService userService;

        public InitializationController(IMembership membership, IUserService userService)
        {
            this.membership = membership;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("initialization")]
        public IHttpActionResult Warmup()
        {
            try
            {
                var restClient = new RestClient(ConfigurationManager.AppSettings["WARMUP_URL"]);

                var formBody = ConfigurationManager.AppSettings["WARMUP_DATASOURCE_BODY"];

                var request = new RestRequest(string.Format("/datasource/bizUsers"), Method.POST);

                request.AddHeader("AuthToken", ConfigurationManager.AppSettings["WARMUP_AUTH_HEADER"]);

                foreach (var item in formBody.Split('&'))
                {
                    request.AddParameter(item.Split('=')[0], item.Split('=')[1]);
                }

                var response = restClient.Execute(request);

                return Ok(new { response.Content });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*[AllowAnonymous]
        [Route("initialization")]
        public IHttpActionResult Get()
        {
            this.userService.QueryUsers().FirstOrDefault();
            this.membership.WarmUp();

            return Ok();
        }*/
    }
}