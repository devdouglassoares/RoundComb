
using RoundComb.Commons;
using RoundComb.ServicesProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Configuration;

namespace RoundComb.ServiceLayer.SoapApi
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SoapApi : ISoapApi
    {
        /*
        #region Public Methods

        public RespostaContract<DocumentoContract> getDocumento(string idMsgEntity, string fileName)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, fileName = fileName }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<DocumentoContract> resposta = new RespostaContract<DocumentoContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getDocumento(fileName);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

                dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro }, documento = resposta.entidade == null ? null :new { fileName = resposta.entidade.Filename, infoTecnica = resposta.entidade.infoTecnica, lenBytes = resposta.entidade.Bytes.LongLength } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;            
        }


        public RespostaContract<MessageExternalIdentificadorContract> getMessageValidation(string idMsgEntity, MessageExternalContract msg)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, message = new { msgId = msg.msgId, msgDate = msg.msgDate, msgSenderId = msg.msgSenderId, msgReceiverId = msg.msgReceiverId, msgTypeId = msg.msgTypeId } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<MessageExternalIdentificadorContract> resposta = new RespostaContract<MessageExternalIdentificadorContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getValidationMsgBody(idMsgEntity, msg);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro }, msgId = resposta.entidade == null ? null : resposta.entidade.msgId }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);

            return resposta;   
        }

        public RespostaContract<MessageExternalIdentificadorContract> setMessageSubmit(string idMsgEntity, MessageExternalContract msg)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, message = new { msgId = msg.msgId, msgDate = msg.msgDate, msgSenderId = msg.msgSenderId, msgReceiverId = msg.msgReceiverId, msgTypeId = msg.msgTypeId, msgBody = msg.msgBody.ToString() } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<MessageExternalIdentificadorContract> resposta = new RespostaContract<MessageExternalIdentificadorContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.setSubmitMsgBody(idMsgEntity, msg);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            if (resposta.entidade != null)
                if (resposta.entidade.idQueue == 0 && resposta.idErro == 0)
                {
                    resposta.idErro = -1;
                    resposta.msgErro = "Não foi possível processar o pedido";
                }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro }, msgId = resposta.entidade == null ? null : resposta.entidade.msgId }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);

            return resposta;  

        }


        public RespostaContract<SimpleMessageContract> isAlive(string idMsgEntity)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<SimpleMessageContract> resposta = new RespostaContract<SimpleMessageContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.isAlive();
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;      
        }

        
        public RespostaContract<SimpleMessageContract> getMessageCompleteXsd(string idMsgEntity)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<SimpleMessageContract> resposta = new RespostaContract<SimpleMessageContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getMessageExternalXsd();
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);

            return resposta;  
        }


        public RespostaContract<SimpleMessageContract> getMsgBodyXsd(string idMsgEntity, int msgTypeId)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<SimpleMessageContract> resposta = new RespostaContract<SimpleMessageContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider. getExternalMessageTypeXsd(msgTypeId, idMsgEntity);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);

            return resposta;
        }

        public RespostaContract<SimpleMessageContract> getMsgBodySimplifiedXsd(string idMsgEntity, int msgTypeId)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<SimpleMessageContract> resposta = new RespostaContract<SimpleMessageContract>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getExternalMessageTypeSimplifiedXsd(msgTypeId, idMsgEntity);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);

            return resposta;
        }

        public RespostaContract<List<TribunalExternalContract>> getTribunaisList(string idMsgEntity)
        {

            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<List<TribunalExternalContract>> resposta = new RespostaContract<List<TribunalExternalContract>>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getTribunaisList();
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;      

        }

        public RespostaContract<List<UnidadeOrganicaExternalContract>> getUnidadesOrganicasListByIdTribunal(string idMsgEntity, int idTribunal)
        {

            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, idTribunal = idTribunal }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<List<UnidadeOrganicaExternalContract>> resposta = new RespostaContract<List<UnidadeOrganicaExternalContract>>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getUnidadesOrganicasListByIdTribunal(idTribunal);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;

        }

        public RespostaContract<string> getLocalizarProcesso(string idMsgEntity, string processo,string guidprocesso, string nifInterv)
        {

            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, processo = processo, guidprocesso = guidprocesso, nifInterv = nifInterv }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<string> resposta = new RespostaContract<string>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getLocalizarProcesso(processo, guidprocesso, nifInterv);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;
        }
        public RespostaContract<string> getEstadoComunicacaoProcesso(string idMsgEntity, string idMsg)
        {

            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, idMsg = idMsg }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<string> resposta = new RespostaContract<string>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getEstadoComunicacaoProcesso(idMsg);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;
        }

        public RespostaContract<string> getEstadoDistribuicaoProcesso(string idMsgEntity, string processo)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, processo = processo }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<string> resposta = new RespostaContract<string>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getEstadoDistribuicaoProcesso(processo);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;
        }

         public RespostaContract<string> getEstadoProcesso(string idMsgEntity, string guidprocesso)
        {
            string idRequest = Guid.NewGuid().ToString();
            string dataSerialized = JsonConvert.SerializeObject(new { idMsgEntity = idMsgEntity, guidprocesso = guidprocesso }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_REQUEST, idRequest, getCurrentMethod(), dataSerialized);
            Log.Instance.Info(Constants.StringFormaters.DEBUG, idRequest, getCurrentMethod(), "Invocado por idMsgEntity = " + idMsgEntity.ToString());

            RespostaContract<string> resposta = new RespostaContract<string>();

            if (ServiceValidator.isClientAuthorizedToCallWsMethod(idMsgEntity, getCurrentMethod(), getCurrentEndPoint()))
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                resposta = serviceProvider.getEstadoProcesso(guidprocesso);
            }
            else
            {
                resposta.idErro = Constants.Errors.ErrorNumbers.NOT_AUTHORIZED_TO_CALL_METHOD;
                resposta.msgErro = Constants.Errors.ErrorMessages.NOT_AUTHORIZED_TO_CALL_METHOD;
            }

            dataSerialized = JsonConvert.SerializeObject(new { erro = new { idErro = resposta.idErro, msgErrro = resposta.msgErro } }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.Indented });
            Log.Instance.Trace(Constants.StringFormaters.TRACE_RESPONSE, idRequest, getCurrentMethod(), dataSerialized);


            return resposta;
        }
   

        #endregion


        #region Private Commons

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string getCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string getCurrentEndPoint()
        {

            string endpointname = ConfigurationManager.AppSettings.Get("CITIUSWsEndPoint");

            return endpointname;

        }

        #endregion
        */
    }
}
