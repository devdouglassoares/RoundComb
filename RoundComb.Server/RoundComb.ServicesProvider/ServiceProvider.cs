using System;
using System.Collections.Generic;
using RoundComb.Commons.Models;
using RoundComb.Commons.ExceptionHandling;
using RoundComb.Commons;

namespace RoundComb.ServicesProvider
{
    public class ServiceProvider
    {

        #region SignatureRequest

        public RespostaContract<string> getSignUrl(SignatureRequestModel signaturerequest)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getSignUrl(signaturerequest);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETSIGNURL;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        public RespostaContract<string> setSignatureDocumentDone(string guidchatroom, string signerRole)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.setSignatureDocumentDone(guidchatroom, signerRole);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETSIGNURL;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        public RespostaContract<string> setHelloSignTemplateToNewProperty(string editurl, int propertyID)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.setHelloSignTemplateToNewProperty(editurl, propertyID);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETSIGNURL;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        

        public RespostaContract<string> setPropertyContractTemplate(PropertyContractTemplate newcontracttempate)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.setPropertyContractTemplate(newcontracttempate);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETSIGNURL;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }
        

        public RespostaContract<string> getSignatureDocument(string guidchatroom)
        {
           
            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            RespostaContract<string> resposta = new RespostaContract<string>();

            try
            {
                resposta = dataProvider.getSignatureDocument(guidchatroom);

            }
            catch (Exception ex)
            {
                resposta.msgErro = ex.Message;
                //ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

            }

            return resposta;
        }
        

        #endregion SignatureRequest

        #region Event controller

        public RespostaContract<string> createNewChatRoom(ChatRoomModel chatmodel)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.createNewChatRoom(chatmodel);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_CREATENEWEVENT;
                resposta.msgErro = ex.Message;
            }

            return resposta;

        }

        public RespostaContract<string> GetEditTemplateUrl(string templateid)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                return dataProvider.GetEditTemplateUrl(templateid);

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));
                /*
                 * // atribuir aqui erros
                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = Constants.Errors.ErrorMessages.ERROR_GETLISTOFEVENTS;
                */
            }

            return resposta;

        }
        

        public RespostaContract<string> GetMyListOfEvents(int iduser)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                return dataProvider.GetMyListOfEvents(iduser);

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = Constants.Errors.ErrorMessages.ERROR_GETLISTOFEVENTS;
            }

            return resposta;

        }

        public RespostaContract<string> GetMyListOfEventsCount(int iduser)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                return dataProvider.GetMyListOfEventsCount(iduser);

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = Constants.Errors.ErrorMessages.ERROR_GETLISTOFEVENTS;
            }

            return resposta;

        }

        public RespostaContract<string> setUnreadMessage(UnreadMessageParam setmsg)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                return dataProvider.setUnreadMessage(setmsg);

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                //resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = ex.Message;
            }

            return resposta;

        }

        public RespostaContract<string> getCountUnReadMessages(string userid)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getCountUnReadMessages(userid);
                

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                //resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = ex.Message;
            }

            return resposta;

        }
        

        #endregion Event controller

        #region Properties controller

        public RespostaContract<string> GetMyListOfmyProperties(int iduser)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            /*DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                return dataProvider.GetMyListOfmyProperties(iduser);

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = Constants.Errors.ErrorMessages.ERROR_GETLISTOFEVENTS;
            }
            */
            return resposta;

        }

        public RespostaContract<string> InformOwnerNewProperty(int propertyid)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            /*DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                return dataProvider.GetMyListOfmyProperties(iduser);

            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_GETLISTOFEVENTS;
                resposta.msgErro = Constants.Errors.ErrorMessages.ERROR_GETLISTOFEVENTS;
            }
            */
            return resposta;

        }
        #endregion Properties controller

        #region global

        public RespostaContract<string> setUserClick(UserclickInfo userclick)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.setUserClick(userclick);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        public RespostaContract<string> createNewContractDocument(ContractDocument contract)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.createNewContractDocument(contract);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }
        

        public RespostaContract<string> getUserClickViewPropertyDetails(string listofproperties)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getUserClickViewPropertyDetails(listofproperties);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        public RespostaContract<string> getMyProperties(string ownerid)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getMyProperties(ownerid);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        public RespostaContract<string> getContractTemplatesDocuments(string eventid)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getContractTemplatesDocuments(eventid);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        public RespostaContract<string> getContractByPropertyId(string propertyId, string eventId)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getContractByPropertyId(propertyId, eventId);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }


        public RespostaContract<string> getContractReplacedByPropertyId(string propertyId, string eventId,string HTMLContent)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();

            DataProvider.DataProvider dataProvider = new DataProvider.DataProvider();

            try
            {
                resposta = dataProvider.getContractReplacedByPropertyId(propertyId, eventId, HTMLContent);
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                resposta.idErro = Constants.Errors.ErrorNumbers.ERROR_SETUSERCLICK;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }


        #endregion global
    }

}
