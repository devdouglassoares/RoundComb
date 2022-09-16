using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using RoundComb.Commons.Models;
using RoundComb.ServicesProvider;

namespace RoundComb.ServiceLayer.RESTApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PropertyController : ApiController
    {
        private readonly ServiceProvider _serviceprovider;
        public PropertyController()
        {
            _serviceprovider = new ServiceProvider();

        }


        [HttpGet]
        [Route("~/api/property/getlistofmyproperties/{userid}")]
        public HttpResponseMessage GetMyListOfmyPropertie(int userid)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.GetMyListOfmyProperties(userid);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("~/api/property/InformOwnerNewProperty/{propertyid}")]
        public HttpResponseMessage InformOwnerNewProperty(int propertyid)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.InformOwnerNewProperty(propertyid);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}