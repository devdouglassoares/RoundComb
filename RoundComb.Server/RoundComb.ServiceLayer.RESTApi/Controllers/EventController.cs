using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Script.Serialization;
using RoundComb.Commons.Models;
using RoundComb.ServicesProvider;

namespace RoundComb.ServiceLayer.RESTApi.Controllers
{
    public class EventController : ApiController
    {

        private readonly ServiceProvider _serviceprovider;
        public EventController()
        {
            _serviceprovider = new ServiceProvider();

        }

        [HttpPost, HttpHead]
        [Route("~/api/event/createchatroom")]
        public HttpResponseMessage Createchatroom(ChatRoomModel chatroom)
        {
            try
             {
                 RespostaContract<string> resposta = _serviceprovider.createNewChatRoom(chatroom);
                
                 if(resposta.idErro == 0)
                     return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                 else
                     return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);
                 
             }
             catch
             {
                 return Request.CreateResponse(HttpStatusCode.InternalServerError);
             }
        }

        [HttpGet]
        [Route("~/api/event/getmylistofevents/{id}")]
        public HttpResponseMessage GetMyListOfEvents(int id)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.GetMyListOfEvents(id);

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

        [HttpGet]
        [Route("~/api/event/getmylistofeventsCount/{id}")]
        public HttpResponseMessage GetMyListOfEventsCount(int id)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.GetMyListOfEventsCount(id);

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

        [HttpPost, HttpHead]
        [Route("~/api/getSignUrl")]
        public HttpResponseMessage getSignUrl(SignatureRequestModel signaturerequest)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.getSignUrl(signaturerequest);


                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);

 
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [HttpGet]
        [Route("~/api/GetEditTemplateUrl/{templateid}")]
        public HttpResponseMessage setEditTemplate(string templateid)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.GetEditTemplateUrl(templateid);


                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("~/api/setPropertyContractTemplate")]
        public HttpResponseMessage setPropertyContractTemplate(PropertyContractTemplate newcontracttempate)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.setPropertyContractTemplate(newcontracttempate);


                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost, HttpHead]
        [Route("~/api/setSignatureDocumentDone")]
        public HttpResponseMessage setSignatureDocumentDone(SignatureRequestModel signaturerequest)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.setSignatureDocumentDone(signaturerequest.guidchatroom.ToString(), signaturerequest.mysignerRole);


                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("~/api/setHelloSignTemplateToNewProperty")]
        public HttpResponseMessage setHelloSignTemplateToNewProperty(AssociateTemplateToProperty template)
        {
            try
            {
                RespostaContract<string> resposta = new RespostaContract<string>();
                int propertyid = 0;

                if(int.TryParse(template.propertyID,out propertyid))
                    resposta = _serviceprovider.setHelloSignTemplateToNewProperty(template.editURL, propertyid);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, -1, "Erro to convert propertyID to integer.");

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("~/api/getSignatureDocument/{guidchatroom}")]
        public HttpResponseMessage getSignatureDocument(string guidchatroom)
        {
            try
            {
                string filename = string.Empty;

                RespostaContract<string> resposta = new RespostaContract<string>();

                resposta = _serviceprovider.getSignatureDocument(guidchatroom);


                if(string.IsNullOrEmpty(resposta.msgErro))
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.msgErro);


                /* var result = new HttpResponseMessage(HttpStatusCode.OK)
                 {
                     Content = new ByteArrayContent(content)
                 };

                 result.Content.Headers.ContentDisposition =
                     new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                     {
                         FileName = filename
                     };

                 result.Content.Headers.ContentType =
                     new MediaTypeHeaderValue("application/octet-stream");

                 return Request.CreateResponse(HttpStatusCode.OK, result);
               */
                /*
                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);

                */
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
           
        }

        [HttpGet]
        [Route("~/api/setReadedMessage")]
        public HttpResponseMessage setUnreadMessage(UnreadMessageParam setmsg)
        {
            try
            {
                string filename = string.Empty;

                RespostaContract<string> resposta = new RespostaContract<string>();

                resposta = _serviceprovider.setUnreadMessage(setmsg);


                if (string.IsNullOrEmpty(resposta.msgErro))
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.msgErro);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("~/api/getCountUnReadMessages/{userid}")]
        public HttpResponseMessage getCountUnReadMessages(string userid)
        {
            try
            {
                string filename = string.Empty;

                RespostaContract<string> resposta = new RespostaContract<string>();

                resposta = _serviceprovider.getCountUnReadMessages(userid);


                if (string.IsNullOrEmpty(resposta.msgErro))
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.msgErro);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost, HttpHead]
        [Route("~/api/setUserClick")]
        public HttpResponseMessage setUserClick(UserclickInfo userclick)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.setUserClick(userclick);


                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("~/api/getUserViewCountPropertyDetails/{listofproperties}")]
        public HttpResponseMessage getUserViewCountPropertyDetails(string listofproperties)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.getUserClickViewPropertyDetails(listofproperties);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("~/api/myProperties/{ownerid}")]
        public HttpResponseMessage getMyProperties(string ownerid)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.getMyProperties(ownerid);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("~/api/getContractTemplatesDocuments/{eventid}")]
        public HttpResponseMessage getContractTemplatesDocuments(string eventid)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.getContractTemplatesDocuments(eventid);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost, HttpHead]
        [Route("~/api/createNewContractDocument")]
        public HttpResponseMessage createNewContractDocument(ContractDocument contract)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.createNewContractDocument(contract);


                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("~/api/getContractByPropertyId")]
        public HttpResponseMessage getContractByPropertyId(getContractByPropertyId reqfilter)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.getContractByPropertyId(reqfilter.IdProperty, reqfilter.IdEvent);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("~/api/getContractReplacedByPropertyId")]
        public HttpResponseMessage getContractReplacedByPropertyId(getContractByPropertyId reqfilter)
        {
            try
            {
                RespostaContract<string> resposta = _serviceprovider.getContractReplacedByPropertyId(reqfilter.IdProperty, reqfilter.IdEvent, reqfilter.HTMLContent);

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("~/api/convertHTMLToPDF")]
        public HttpResponseMessage convertHTMLToPDF(HTMLContentToConvert HTMLContent)
        {
            try
            {

                byte[] res = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(HTMLContent.HTMLContent, PdfSharp.PageSize.A4);
                    pdf.Save(ms);
                    res = ms.ToArray();
                } 

                RespostaContract<string> resposta = new RespostaContract<string>();
               
                resposta.entidade = Convert.ToBase64String(res, 0, res.Length);

                /*
                char[] chars = new char[res.Length / sizeof(char)];
                System.Buffer.BlockCopy(res, 0, chars, 0, res.Length);

                RespostaContract<string> resposta = new RespostaContract<string>();

                resposta.entidade = new string(chars);
                */

                if (resposta.idErro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, resposta.entidade);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, resposta.idErro, resposta.msgErro);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException);
            }
        }
    }

}
