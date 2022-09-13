using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using RoundComb.Commons.Models;
using RoundComb.WSAccess;

namespace RoundComb.DataProvider
{
    public class DataProvider
    {
        #region Init

        public DataProvider()
        {
        }

        #region SignatureRequest

        public RespostaContract<string> getSignUrl(SignatureRequestModel signaturerequest)
        {
 
            WSAccess.WSAccess wsAccess = new WSAccess.WSAccess();

            return wsAccess.getSignUrl(signaturerequest);

        }

        public RespostaContract<string> setSignatureDocumentDone(string guidchatroom, string signerRole)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            return dataAccess.setSignatureDocumentDone(guidchatroom, signerRole);

        }

        public RespostaContract<string> setHelloSignTemplateToNewProperty(string editurl, int propertyID)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            return dataAccess.setHelloSignTemplateToNewProperty(editurl, propertyID);
        }

        public RespostaContract<string> setPropertyContractTemplate(PropertyContractTemplate newcontracttempate)
        {

            RespostaContract<string> resp = new RespostaContract<string>();

            WSAccess.WSAccess wsAccess = new WSAccess.WSAccess();
            resp = wsAccess.setPropertyContractTemplate(newcontracttempate);

            return resp;

        }

        public RespostaContract<string>  getSignatureDocument(string guidchatroom)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> respSignatureRequestId = new RespostaContract<string>();

            respSignatureRequestId = dataAccess.getSignatureRequestId(guidchatroom);

            if (string.IsNullOrEmpty(respSignatureRequestId.msgErro))
            {
                WSAccess.WSAccess wsAccess = new WSAccess.WSAccess();

                return wsAccess.DownloadSignatureRequestFile(respSignatureRequestId.entidade);
            }
            else
                return respSignatureRequestId;

        }

        public RespostaContract<string> GetEditTemplateUrl(string templateid)
        {
        
            WSAccess.WSAccess wsAccess = new WSAccess.WSAccess();

            return wsAccess.GetEditTemplateUrl(templateid);
   
        }
        

        #endregion SignatureRequest

        #endregion


        public RespostaContract<string> createNewChatRoom(ChatRoomModel chatmodel)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            var json = new JavaScriptSerializer().Serialize(chatmodel);
            return  dataAccess.createNewChatRoom(json);

        }

        public RespostaContract<string> GetMyListOfEvents(int iduser)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
            RespostaContract<List<ListOfEventsModel>> resptemp = new RespostaContract<List<ListOfEventsModel>>();

            resptemp = dataAccess.GetMyListOfEvents(iduser);

            RespostaContract<string> resposta = new RespostaContract<string>();

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }

        public RespostaContract<string> GetMyListOfEventsCount(int iduser)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
            RespostaContract<List<ListOfEventsCountModel>> resptemp = new RespostaContract<List<ListOfEventsCountModel>>();

            resptemp = dataAccess.GetMyListOfEventsCount(iduser);

            RespostaContract<string> resposta = new RespostaContract<string>();

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }

        public RespostaContract<string> setUnreadMessage(UnreadMessageParam setmsg)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resposta = dataAccess.setUnreadMessage(setmsg);

            return resposta;

        }

        public RespostaContract<string> getCountUnReadMessages(string userid)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resposta = dataAccess.getCountUnReadMessages(userid);

            return resposta;

        }

        #region global

        public RespostaContract<string> setUserClick(UserclickInfo userclick)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resposta = dataAccess.setUserClick(userclick);

            return resposta;

        }

        public RespostaContract<string> createNewContractDocument(ContractDocument contract)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resposta = dataAccess.createNewContractDocument(contract);

            return resposta;

        }
        

        public RespostaContract<string> getUserClickViewPropertyDetails(string listofproperties)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<List<UserClickViewsPerProperty>> resptemp = new RespostaContract<List<UserClickViewsPerProperty>>();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resptemp = dataAccess.getUserClickViewPropertyDetails(listofproperties);

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }

        public RespostaContract<string> getMyProperties(string ownerid)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<List<ListOfMyProperties>> resptemp = new RespostaContract<List<ListOfMyProperties>>();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resptemp = dataAccess.getMyProperties(ownerid);

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }

        public RespostaContract<string> getContractTemplatesDocuments(string eventid)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<List<ContractTemplateDocument>> resptemp = new RespostaContract<List<ContractTemplateDocument>>();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resptemp = dataAccess.getContractTemplatesDocuments(eventid);

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }

        public RespostaContract<string> getContractByPropertyId(string propertyId, string eventId)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> resptemp = new RespostaContract<string>();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resptemp = dataAccess.getContractByPropertyId(propertyId, eventId);

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }
        public RespostaContract<string> getContractReplacedByPropertyId(string propertyId, string eventId, string HTMLContent)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            RespostaContract<string> resptemp = new RespostaContract<string>();

            RespostaContract<string> resposta = new RespostaContract<string>();

            resptemp = dataAccess.getContractReplacedByPropertyId(propertyId, eventId, HTMLContent);

            resposta.entidade = new JavaScriptSerializer().Serialize(resptemp.entidade);
            resposta.idErro = resptemp.idErro;
            resposta.msgErro = resptemp.msgErro;

            return resposta;

        }
       

        #endregion golobal


        /*
                #region Commons

                public Documento getDocumento(string fileName)
                {
                    Documento documento = new Documento();

                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    byte[] documentBytes = null;

                    try
                    {
                        documentBytes = dataAccess.getDocumento(fileName);
                    }
                    catch (Exception ex)
                    {
                        documento.idErro = Constants.Errors.ErrorNumbers.ERRO_LEITURA_DOCUMENTO_DO_GESTOR_DOCUMENTAL;
                        documento.msgErro = Constants.Errors.ErrorMessages.ERRO_LEITURA_DOCUMENTO_DO_GESTOR_DOCUMENTAL + " - " + ex.Message;
                    }

                    if (documento.idErro == 0)
                    {
                        documento.Filename = fileName;
                        documento.infoTecnica = Helper.getFileNameWithoutExtension(fileName);
                        documento.Bytes = documentBytes;
                    }            

                    return documento;
                }

                private TransactionOptions getTransactionOptions()
                {
                    return new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.MaxValue };
                }

                public int External_Message_Get_Ato_IDs(int idAto, int getType, int idExternalEntity)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.External_Message_Get_Ato_IDs(idAto, getType, idExternalEntity);
                }

                public MessageQueueManagerModel MessageQueueGetRowByIDQueue(int idQueue)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.MessageQueueGetRowByIDQueue(idQueue);
                }

                public PaisExternalModel getPaisByIsoPais(string isoPais)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getPaisByIsoPais(isoPais);
                }

                public List<ExternalMessageEntitiesProcessBehaviours> ExternalMessageEntitiesProcessBehavioursManagerGetAll(int idExternalEntity, int idMsgType)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.ExternalMessageEntitiesProcessBehavioursManagerGetAll(idExternalEntity, idMsgType);
                }

                public ExternalMessageEntitiesProcessBehaviours ExternalMessageEntitiesProcessBehavioursManagerGetByIdSp(int idExternalEntity, int idMsgType, int idSP)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.ExternalMessageEntitiesProcessBehavioursManagerGetByIdSp(idExternalEntity, idMsgType, idSP);
                }

                public ExternalMessageEntitiesProcessBehaviours ExternalMessageEntitiesProcessBehavioursManagerGetBySpName(int idExternalEntity, int idMsgType, string spName)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.ExternalMessageEntitiesProcessBehavioursManagerGetBySpName(idExternalEntity, idMsgType, spName);
                }

                public bool IsMsgTypeAtivo(int idMsgType)
                {
                     bool IsAtivo = false;

                    try 
                    {	        
                        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                        MessageType msgType = dataAccess.GetMsgType(idMsgType);

                        IsAtivo = msgType.isAtivo;
                    }
                    catch (Exception)
                    {		
                    }

                    return IsAtivo;
                }

                public Tuple<int, string> getEstadoComunicacaoProcesso(string idMsg)
                {

                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    Tuple<int, string> tupleresposta = null;

                    try
                    {
                        MessageRowModel resp = dataAccess.getEstadoComunicacaoProcesso(idMsg);

                        // não existe nada na queue
                        if (resp == null)
                        {
                            tupleresposta = new Tuple<int, string>(Constants.EstadoComunicacaoProcesso.NAO_RECEBIDO, Constants.Messages.NAO_RECEBIDO);
                        }
                        else // erro ao tentar obter o estado da comunicação do processo
                            if(resp.idErro == -1)
                            {
                                tupleresposta = new Tuple<int, string>(Constants.EstadoComunicacaoProcesso.ESTADO_COMUNICACAO_DESCONHECIDO, Constants.Messages.ESTADO_COMUNICACAO_DESCONHECIDO);
                            }
                            else
                            {   
                                if(resp.isDone == true)
                                    tupleresposta = new Tuple<int, string>(Constants.EstadoComunicacaoProcesso.RECEBIDO_ENTREGUE_TRIBUNAL, Constants.Messages.RECEBIDO_ENTREGUE_TRIBUNAL);
                                else // foi recebido, mas não entregue no tribunal
                                    tupleresposta = new Tuple<int, string>(Constants.EstadoComunicacaoProcesso.RECEBIDO_NAO_ENTREGUE_TRIBUNAL, Constants.Messages.RECEBIDO_NAO_ENTREGUE_TRIBUNAL);
                            }
                     }
                    catch(Exception ex)
                    {
                        tupleresposta = new Tuple<int, string>(Constants.EstadoComunicacaoProcesso.ESTADO_COMUNICACAO_DESCONHECIDO, Constants.Messages.ESTADO_COMUNICACAO_DESCONHECIDO);
                    }

                    return tupleresposta;
                }


                public RespostaContract<MessageLocalizarProcessoModel> getLocalizarProcesso(string processo, string guidprocesso, string nifInterv)
                {

                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getLocalizarProcesso(processo, guidprocesso, nifInterv);

                }
                public ProcessoCacheModel getEstadoDistribuicaoProcesso(string processo)
                {

                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getEstadoDistribuicaoProcesso(processo);

                }


                public EstadoProcessoCacheModel getEstadoProcesso(string guidprocesso)
                {

                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getEstadoProcesso(guidprocesso);

                }

                #endregion

                public List<TribunalExternalModel> getTribunaisList()
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getTribunaisList(Constants.Operations.TiposLista.TRIBUNAIS_PARA_INTEROPERABILIDADE_UNSMAPA_BASE);
                }

                public List<UnidadeOrganicaExternalModel> getUnidadesOrganicasListByIdTribunal(int idTribunal)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getUnidadesOrganicasListByIdTribunal(idTribunal, Constants.Operations.TiposLista.UNIDADES_ORGANICAS_PARA_INTEROPERABILIDADE_UNSMAPA_BASE);
                }


                #region Message Queue

                public void MessageQueueError(int idQueue, int idErro, string msgErro)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    dataAccess.MessageQueueError(idQueue, idErro, msgErro);
                }

                #endregion


                #region DocsMigration

                public void migrateDocs()
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<ServidoresModel> servidores = dataAccess.CitiusNextGetServidoresLista();

                    foreach (ServidoresModel servidor in servidores)
                    {
                       //Task.Factory.StartNew(() => migraSingleTribunal(servidor.idTribRef));
                       migraSingleTribunal(servidor.idTribRef);

                    }
                }

                private void migraSingleTribunal(int idTribRef)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<MessageQueueManagerModel> lstRegsToMigrate = new List<MessageQueueManagerModel>();
                    lstRegsToMigrate = dataAccess.MigrationOrqGetRegsNotMigrated(idTribRef);
                    foreach (MessageQueueManagerModel message in lstRegsToMigrate)
                    {
                       //if (message.idQueue == 1018291)
                        if (IsMsgTypeAtivo(message.idMsgType))
                        {
                            migrateMessageQueue(message);
                        }
                    }
                }

                private void migrateMessageQueue(MessageQueueManagerModel message)
                {
                    string pKey = ConfigurationManager.AppSettings.Get("key");
                    string erro = string.Empty;
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    bool isMigrated = true;
                    List<ActoDocumentoModel> lstDocsToMigrate = getDocsToMigrate(message.idTribRef, message.idRegisto, message.idMsgType);
                    foreach (ActoDocumentoModel doc in lstDocsToMigrate)
                    {
                        if (isMigrated)
                        {
                            Gestor.Documental.Model.DocumentoModel docGestor = migraDoc(message.idTribRef, message.idQueue, doc, pKey);
                            if (docGestor.idErro != 0)
                            {
                                isMigrated = false;
                                erro = docGestor.msgErro;
                            }
                            else
                            {
                                dataAccess.MigrationTribUpdateMigratedDoc(message.idTribRef, doc.idDoc, docGestor.idServidor, docGestor.idDocGestor, docGestor.chaveDoc);
                            }
                        }
                    }

                    if (isMigrated)
                    {
                        dataAccess.MigrationOrqUpdateRegMigrated(message.idQueue);
                    }
                    else
                    {
                        dataAccess.MessageQueueError(message.idQueue, -1, erro);
                    }
                }

                private Gestor.Documental.Model.DocumentoModel migraDoc(int idTribRef, int idQueue, ActoDocumentoModel doc, string pKey)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

                    doc = dataAccess.MigrationTribGetDocBin(idTribRef, doc, pKey);


                    Gestor.Documental.Business.GestorDocumentalBusiness gestor = new Gestor.Documental.Business.GestorDocumentalBusiness();
                    Gestor.Documental.Model.DocumentoModel docGestor = new Gestor.Documental.Model.DocumentoModel();
                    docGestor.extensaoDoc = ".pdf";
                    docGestor.idUtil = 1;
                    docGestor.utilizador = "Externals";
                    docGestor.chaveDoc = doc.chaveDoc;
                    docGestor.binarioDoc = doc.binDoc;

                    try
                    {
                        docGestor = gestor.gestDocWriteDocumento(docGestor, pKey);
                        if (docGestor.idErro != 0)
                        {
                            throw new Exception("idQueue = " + idQueue + " " + docGestor.msgErro);
                        }
                    }
                    catch (Exception ex)
                    {
                        docGestor.idErro = -1;
                        docGestor.msgErro = ex.Message;
                        RoundComb.ExceptionHandling.ExceptionManager.HandleException(typeof(string), ex);
                    }

                    return docGestor;
                }

                private List<ActoDocumentoModel> getDocsToMigrate(int idTribRef, int nActo, int idMsgType)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<ActoDocumentoModel> lstDocsToMigrate = new List<ActoDocumentoModel>();
                    List<ActoProcessualModel> atos = dataAccess.MigrationTribGetDocsToMigrate(idTribRef, nActo, idMsgType);
                    if (atos.Count > 0)
                    {
                        lstDocsToMigrate = atos[0].lstActoDocumentos;
                        List<ActoAnexoModel> anexos = new List<ActoAnexoModel>();
                        anexos = atos[0].lstActoAnexos;
                        if (anexos.Count > 0)
                        {
                            foreach (ActoAnexoModel anexo in anexos)
                            {
                                ActoDocumentoModel actoAnexo = new ActoDocumentoModel();
                                actoAnexo.idDoc = anexo.idDoc;
                                actoAnexo.idServidor = anexo.idServidor;
                                actoAnexo.idDocGestor = anexo.idDocGestor;
                                actoAnexo.idNacional = anexo.idNacional;
                                lstDocsToMigrate.Add(actoAnexo);
                            }
                        }
                    }

                    lstDocsToMigrate = lstDocsToMigrate.FindAll(x => x.idServidor <= 0);

                    return lstDocsToMigrate;
                }

                #endregion DocsMigration


                #region BuildMsgBody

                public void buildMsgBody(int idInternalEntity)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<ServidoresModel> servidores = dataAccess.CitiusNextGetServidoresLista();

                    foreach (ServidoresModel servidor in servidores)
                    {
                       //Task.Factory.StartNew(() => buildMsgBodySingleTribunal(idInternalEntity, servidor.idTribRef));        
                       buildMsgBodySingleTribunal(idInternalEntity, servidor.idTribRef);
                    }
                }

                private void buildMsgBodySingleTribunal(int idInternalEntity, int idTribRef)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<MessageQueueManagerModel> lstRegsToBuildMsgBody = new List<MessageQueueManagerModel>();
                    lstRegsToBuildMsgBody = dataAccess.BuildMsgBodyGetRegsNotBuilt(idTribRef);
                    foreach (MessageQueueManagerModel message in lstRegsToBuildMsgBody)
                    {
                        if (IsMsgTypeAtivo(message.idMsgType))
                        {
                            message.idExternalEntity = idInternalEntity;
                            buildMsgBodyMessageQueue(message);
                        }
                    }
                }

                private void buildMsgBodyMessageQueue(MessageQueueManagerModel message)
                {            
                    string erro = string.Empty;
                    bool isBuilt = true;
                    string msgBody = string.Empty;
                    string partes = string.Empty;

                    string pKey = ConfigurationManager.AppSettings.Get("key");
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

                    try
                    {
                        MessageType msgType = dataAccess.BuildMsgBodyGetSPForTrib(message.idMsgType);

                        if (msgType.idErro != 0)
                        {
                            throw new Exception(msgType.msgErro);
                        }

                        msgBody = dataAccess.BuildMsgBodyTribGetData(message.idTribRef, msgType.SP, message.idMsgType, message.idProcesso, message.idRegisto);
                        partes = dataAccess.BuildMsgBodyTribGetProcessoPartes(message.idTribRef, msgType.SP, message.idProcesso);

                        if (string.Compare(msgBody, RoundComb.Commons.Constants.Errors.ErrorMessages.ERRO_BUILD_MSG_BODY_ENVIO_ANULADO) != 0)             
                            msgBody = BuildMsgBodyUpdateAtosSaida(msgBody, message.idMsgType);
                        else
                            throw new Exception(RoundComb.Commons.Constants.Errors.ErrorMessages.ERRO_BUILD_MSG_BODY_ENVIO_ANULADO);

                    }
                    catch (Exception ex)
                    {
                        erro = ex.Message;
                        isBuilt = false;
                    }

                    if (isBuilt)
                    {
                        dataAccess.BuildMsgBodyUpdateRegBuilt(message.idQueue, msgBody, message.idExternalEntity);
                        dataAccess.BuildMsgBodyUpdatePartes(message.idQueue, partes);

                    }
                    else
                    {
                        dataAccess.MessageQueueError(message.idQueue, -1, erro);
                    }
                }

                private string BuildMsgBodyUpdateAtosSaida(string msgBody, int idMsgType)
                {

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(msgBody);

                    XmlNodeList elemList = xdoc.GetElementsByTagName("idAto");

                    string idAto = elemList.Item(0).InnerText;


                    if (!string.IsNullOrEmpty(idAto))
                    {

                        string pKey = ConfigurationManager.AppSettings.Get("key");
                        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

                        List<AtoProcessualMapeamentoExternalModel> map = dataAccess.getAtosSaidaProcessuaisMapeamentoExternalList(idMsgType, int.Parse(idAto));

                        if (map.Count() == 1 && map[0].idAtoMapeado != 0)
                        {
                            xdoc.GetElementsByTagName("idAto").Item(0).InnerText = map[0].idAtoMapeado.ToString();

                            var settings = new XmlWriterSettings();
                            settings.Indent = true;
                            settings.OmitXmlDeclaration = true;

                            using (var stringWriter = new StringWriter())
                            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
                            {
                                xdoc.WriteTo(xmlTextWriter);
                                xmlTextWriter.Flush();
                                return stringWriter.GetStringBuilder().ToString();
                            }
                        }
                    }

                    return msgBody;

                }
                #endregion BuildMsgBody       


                #region Validate and Receive message from externals

                public IBusinessData getValidationMsgBody(string idMsgEntity, MessageExternalModel msg)
                {
                    ValidationResult validationResult = new ValidationResult();

                    IBusinessData businessEntity = null;

                    try
                    {
                        // Step 1: XML Schema Validation
                        validateXmlShema(validationResult, msg);

                        // Step 2 - Business Ids Validation
                        if (validationResult.validationError == 0)
                        {
                            businessEntity = validateReferenceIds(validationResult, idMsgEntity, msg.msgBody, msg.msgTypeId, msg.msgSenderId);
                        }

                        // Step 3 - Business Rules Validation
                        if (validationResult.validationError == 0)
                        {
                            businessEntity = validateBusinessRules(validationResult, businessEntity, msg.msgTypeId, msg.msgSenderId);
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.HandleException(typeof(string), new Exception(ex.Message));

                        validationResult.validationMsg = ex.Message;
                        validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_GENERICO;
                    }

                    if (businessEntity != null && validationResult.validationError != 0)
                    {
                        businessEntity.idErro = validationResult.validationError;
                        businessEntity.msgErro = validationResult.validationMsg;
                    }

                    return businessEntity;
                }

                public ResultModel<MessageExternalModel> setSubmitMsgBody(string idMsgEntity, MessageExternalModel msg)
                {
                    ResultModel<MessageExternalModel> result = new ResultModel<MessageExternalModel>();

                    // validate msgReceiverId msgSenderId Validation
                    //ValidationResult validationResult = validatemsgReceiverIdAndmsgSenderId(msg);

                    //if(validationResult.validationError == 0)
                    //{
                        // validate business
                        IBusinessData businessData = getValidationMsgBody(idMsgEntity, msg);
                        IBusinessCustomization businessCustomization;

                        if (businessData != null)
                        {
                            try
                            {
                                if (businessData.idErro == 0)
                                {

                                    // Business Rules Customization
                                    businessCustomization = GetBusinessCustomizationEntity(msg.msgTypeId, msg.msgSenderId);

                                    if (businessCustomization != null)
                                        msg.msgBody = businessCustomization.addGuidAtostoXML(msg.msgBody);

                                    // -----------------------------------------

                                    ResultModel resultModel = submitDataFromExternal(idMsgEntity, msg, businessData);
                                    if (resultModel.idErro != 0)
                                    {
                                        businessData.idErro = resultModel.idErro;
                                        businessData.msgErro = resultModel.msgErro;
                                    }
                                    else
                                    {
                                        result.idResult = resultModel.idResult;
                                        result.entidade = msg;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                string message = ExceptionManager.HandleException(typeof(string), new Exception(ex.Message)).Message;

                                businessData.idErro = Constants.Errors.ErrorNumbers.ERRO_GENERICO;
                                businessData.msgErro = message;
                            }

                            if (businessData.idErro != 0)
                            {
                                result.idErro = businessData.idErro;
                                result.msgErro = businessData.msgErro;
                            }
                        }
                        else
                        {
                            result.idErro = Constants.Errors.ErrorNumbers.ERRO_VALIDAR_MENSAGEM_PARA_ENTIDADE_EXTERNA;
                            result.msgErro = Constants.Errors.ErrorMessages.ERRO_VALIDAR_MENSAGEM_PARA_ENTIDADE_EXTERNA;
                        }
                    //}
                    //else
                    //{
                    //    result.idErro = validationResult.validationError;
                    //    result.msgErro = validationResult.validationMsg;
                    //}

                    return result;
                }


                private ResultModel submitDataFromExternal(string idMsgEntity, MessageExternalModel msg, IBusinessData businessData)
                {
                    ResultModel ret = new ResultModel();
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    ret = dataAccess.MessageQueueReceiveInsertReg(idMsgEntity, msg, businessData);
                    return ret;
                }

                #endregion


                #region Xsd

                public Tuple<XmlSchema,bool> getExternalMessageTypeXmlSchema(int idMsgType, string idMsgEntity)
                {
                    Tuple<XmlSchema, bool> result = null;

                    TipoMensagemExternalModel tipoMensagemExternalModel = new DataAccess.DataAccess().getExternalMessageTypeByTypeAndEntity(idMsgType, idMsgEntity);

                    if(string.Compare(tipoMensagemExternalModel.msgSchema,"IgnoreXMLSchemaValidation") == 0)
                    {
                        XmlSchema schema = null;
                        result = Tuple.Create(schema, false);
                    }
                    else
                        if (tipoMensagemExternalModel != null && tipoMensagemExternalModel.idErro == Constants.Errors.ErrorNumbers.NO_ERROR)
                        {
                            Type objType = ContentValidation.getTypeOfMsgSchema(tipoMensagemExternalModel.msgSchema);

                            if (objType != null)
                            {
                                XmlSchema schema = Helper.getXmlSchemaFromClass(objType);
                                result = Tuple.Create(schema,true);
                            }
                        }

                    return result;
                }

                public Tuple<XmlSchema, bool> getExternalMessageTypeXmlSchemaSimplified(int idMsgType, string idMsgEntity)
                {
                    Tuple<XmlSchema, bool> result = null;

                    TipoMensagemExternalModel tipoMensagemExternalModel = new DataAccess.DataAccess().getExternalMessageTypeByTypeAndEntity(idMsgType, idMsgEntity);

                    if(string.Compare(tipoMensagemExternalModel.msgSchema,"IgnoreXMLSchemaValidation") == 0)
                    {

                        Type objType = ContentValidation.getTypeOfMsgSchema("ProcessoExternalModelSimplified");

                        if (objType != null)
                        {
                            XmlSchema schema = Helper.getXmlSchemaFromClass(objType);
                            result = Tuple.Create(schema, true);
                        }





                    }
                    /*else
                        if (tipoMensagemExternalModel != null && tipoMensagemExternalModel.idErro == Constants.Errors.ErrorNumbers.NO_ERROR)
                        {
                            Type objType = ContentValidation.getTypeOfMsgSchema(tipoMensagemExternalModel.msgSchema);

                            if (objType != null)
                            {
                                XmlSchema schema = Helper.getXmlSchemaFromClass(objType);
                                result = Tuple.Create(schema,true);
                            }
                        }
                    */
        /*
                    return result;
                }




                public Tuple<string, bool> getExternalMessageTypeXsd(int idMsgType, string idMsgEntity)
                {
                    Tuple<string, bool> result;

                    Tuple<XmlSchema, bool> tuplexmlSchema = getExternalMessageTypeXmlSchema(idMsgType, idMsgEntity);

                    XmlSchema xmlSchema = tuplexmlSchema.Item1;

                    StringWriter schemaWriter = new StringWriter();

                    if (tuplexmlSchema.Item2 == true)
                    {
                        xmlSchema.Write(schemaWriter);

                        result = Tuple.Create(schemaWriter.ToString(), true);
                    }
                    else
                    {
                        result = Tuple.Create("IgnoreXMLSchemaValidation", false);
                    }

                    return result;
                }

                public Tuple<string, bool> getExternalMessageTypeSimplifiedXsd(int idMsgType, string idMsgEntity)
                {
                    Tuple<string, bool> result;

                    Tuple<XmlSchema, bool> tuplexmlSchema = getExternalMessageTypeXmlSchemaSimplified(idMsgType, idMsgEntity);

                    XmlSchema xmlSchema = tuplexmlSchema.Item1;

                    StringWriter schemaWriter = new StringWriter();

                    if (tuplexmlSchema.Item2 == true)
                    {
                        xmlSchema.Write(schemaWriter);

                        result = Tuple.Create(schemaWriter.ToString(), true);
                    }
                    else
                    {
                        result = Tuple.Create("IgnoreXMLSchemaValidation", false);
                    }

                    return result;
                }




                public string getMessageExternalXsd()
                {
                    XmlSchema xmlSchema = null;
                    StringWriter schemaWriter = new StringWriter();


                        Type objType = typeof(MessageExternalModel);

                        xmlSchema = Helper.getXmlSchemaFromClass(objType);
                        xmlSchema.Write(schemaWriter);


                    return schemaWriter.ToString();
                }

                #endregion Xsd


                #region Validation Methods

                private void validateXmlShema(ValidationResult validationResult, MessageExternalModel msg)
                {
                    Tuple<XmlSchemaSet, bool> schemaSetresult = getXmlSchemaSet(msg.msgTypeId, msg.msgSenderId);



                    //nao se valida
                    if (schemaSetresult.Item2 == false)
                    {
                        validationResult.validationError = 0;
                        validationResult.validationMsg = "";
                    }
                    else
                    {
                        XmlSchemaSet schemaSet = schemaSetresult.Item1;

                        XDocument xmlDoc = ContentValidation.getXmlDocumentFromMsgBody(msg.msgBody);

                        xmlDoc.Validate(schemaSet,  (o, e) =>
                            {
                                validationResult.validationMsg += e.Severity + ": ";

                                if (e.Severity == XmlSeverityType.Error || e.Severity == XmlSeverityType.Warning)
                                {
                                    validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_XSD;
                                    validationResult.validationMsg += e.Message + System.Environment.NewLine;
                                }

                            });
                    }
                }

                private ValidationResult validatemsgReceiverIdAndmsgSenderId(MessageExternalModel msg)
                {

                    ValidationResult validationresult = new ValidationResult();
                    UnidadeOrganicaExternalModel unidadeOrganica = new UnidadeOrganicaExternalModel();
                    bool validationsucess = true;
                    int idUnidadeOrganica;

                    // msgReceiverId
                    if (string.IsNullOrEmpty(msg.msgReceiverId))
                        msg.msgReceiverId = "0";

                    if (string.Compare(msg.msgReceiverId,"0")==0)
                        validationsucess = false;
                     else
                         if(Int32.TryParse(msg.msgReceiverId,out idUnidadeOrganica) && validationsucess == true)
                         {
                            unidadeOrganica = getUnidadeOrganicaById(idUnidadeOrganica);

                            if (unidadeOrganica == null || unidadeOrganica.idErro != 0)
                                validationsucess = false;
                        }


                    if (!validationsucess)
                    {
                        validationresult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_RECEIVERID;
                        validationresult.validationMsg = String.Concat(Constants.Errors.ErrorMessages.ERRO_VALIDACAO_RECEIVERID, " ", msg.msgReceiverId);
                        return validationresult;
                    }

                    // msgSenderId
                    if (string.IsNullOrEmpty(msg.msgSenderId))
                        msg.msgSenderId = "0";

                    if (string.Compare(msg.msgSenderId, "0") == 0)
                        validationsucess = false;
                    else
                        if (Int32.TryParse(msg.msgSenderId, out idUnidadeOrganica) && validationsucess == true)
                        {
                            unidadeOrganica = getUnidadeOrganicaById(idUnidadeOrganica);

                            if (unidadeOrganica == null || unidadeOrganica.idErro != 0)
                                validationsucess = false;
                        }


                    if (!validationsucess)
                    {
                        validationresult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_SENDERID;
                        validationresult.validationMsg = String.Concat(Constants.Errors.ErrorMessages.ERRO_VALIDACAO_SENDERID, " ", msg.msgSenderId);
                        return validationresult;
                    }

                     validationresult.validationError = 0;
                     validationresult.validationMsg = "";

                     return validationresult;
                }

                private Tuple<XmlSchemaSet,bool> getXmlSchemaSet(int msgTypeId, string msgSenderId)
                {
                    Tuple<XmlSchemaSet, bool> result = null;

                    // Obtém o schema para validar o msgBody recebido na mensagem
                    XmlSchemaSet schemaSet = new XmlSchemaSet();

                    Tuple<string, bool> msgtypexsd = getExternalMessageTypeXsd(msgTypeId, msgSenderId);

                    if(msgtypexsd.Item2 == true)
                    {
                        string xsdMarkup = msgtypexsd.Item1;

                        schemaSet.Add(Constants.Namespaces.INEROPERABILITY_TARGET_NAMESPACE, XmlReader.Create(new StringReader(xsdMarkup)));

                    }
                    else
                    {
                        result = Tuple.Create(schemaSet, false);
                    }

                        result = Tuple.Create(schemaSet,true);
                    return result;
                }

                private IBusinessData validateBusinessRules(ValidationResult validationResult, IBusinessData businessEntity, int idMsgType, string idMsgTypeEntity)
                {
                    IBusinessValidator businessValidator = GetBusinessValidatorEntity(idMsgType, idMsgTypeEntity);

                    if(businessValidator != null) 
                    {
                        ErrorMsgModel result = businessValidator.validateProcesso(businessEntity);
                        if (result.idErro != 0)
                        {
                            if (validationResult.validationMsg.Length > 0)
                                validationResult.validationMsg += System.Environment.NewLine;

                            validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_REGRAS_NEGOCIO_VIOLADAS;
                            validationResult.validationMsg += System.Environment.NewLine;
                            validationResult.validationMsg += result.msgErro;
                            validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_REGRAS_NEGOCIO_VIOLADAS;
                        }
                    }
                    else
                    {
                        if (validationResult.validationMsg.Length > 0)
                            validationResult.validationMsg += System.Environment.NewLine;

                        validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_REGRAS_NEGOCIO_CONFIGURACAO;
                        validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TIPO_MENSAGEM_OU_ENTIDADE_MSGTYPE + idMsgType.ToString();
                        validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TIPO_MENSAGEM_OU_ENTIDADE_MSGTYPE_ENTITY + idMsgTypeEntity;
                        validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_REGRAS_NEGOCIO_CONFIGURACAO;
                    }

                    businessValidator = null;

                    return businessEntity;
                }

                private IBusinessData validateReferenceIds(ValidationResult validationResult, string idMsgEntity, string msgBody, int idMsgType, string idMsgTypeEntity)
                {
                    IBusinessData businessEntity = GetBusinessEntity(msgBody, idMsgType, idMsgTypeEntity);

                    if (businessEntity != null)
                    {
                        int idExternalEntity = getIdExternalEntity(idMsgTypeEntity);
                        UnidadeOrganicaExternalModel unidadeOrganica = getUnidadeOrganicaById(businessEntity.idUnidadeOrganica);
                        if (unidadeOrganica != null && unidadeOrganica.idErro == 0)
                        {
                            List<PropertyModel> propList = proceedWithValidation(businessEntity, unidadeOrganica.idTribunal, idMsgEntity);

                            if (propList != null && propList.Count > 0)
                            {
                                if (validationResult.validationMsg.Length > 0)
                                    validationResult.validationMsg += System.Environment.NewLine;

                                string invalidIdsString = ContentValidation.invalidIdsToString(propList);

                                validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TABELAS_REFERENCIA;
                                validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TABELAS_REFERENCIA_IDS + invalidIdsString;
                                validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_TABELAS_REFERENCIA;
                            }
                        }
                        else
                        {
                            if (validationResult.validationMsg.Length > 0)
                                validationResult.validationMsg += System.Environment.NewLine;

                            validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_UNIDADE_ORGANICA;
                            validationResult.validationMsg += " " + businessEntity.idUnidadeOrganica.ToString();
                            validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_UNIDADE_ORGANICA;
                        }               
                    }
                    else
                    {
                        if (validationResult.validationMsg.Length > 0)
                            validationResult.validationMsg += System.Environment.NewLine;

                        validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TIPO_MENSAGEM_OU_ENTIDADE;
                        validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TIPO_MENSAGEM_OU_ENTIDADE_MSGTYPE + idMsgType.ToString();
                        validationResult.validationMsg += Constants.Errors.ErrorMessages.ERRO_VALIDACAO_TIPO_MENSAGEM_OU_ENTIDADE_MSGTYPE_ENTITY + idMsgTypeEntity;
                        validationResult.validationError = Constants.Errors.ErrorNumbers.ERRO_VALIDACAO_TIPO_MENSAGEM_OU_ENTIDADE;
                    }            

                    return businessEntity;
                }

                private IBusinessValidator GetBusinessValidatorEntity(int idMsgType, string idMsgTypeEntity)
                {
                    IBusinessValidator res = null;

                    TipoMensagemExternalModel tipoMensagemExternalModel = new DataAccess.DataAccess().getExternalMessageTypeByTypeAndEntity(idMsgType, idMsgTypeEntity);

                    if (tipoMensagemExternalModel != null && !string.IsNullOrEmpty(tipoMensagemExternalModel.msgBusinessValidator))
                    {
                        switch (tipoMensagemExternalModel.msgBusinessValidator)
                        {
                            case RoundComb.Commons.Constants.BusinessValidators.SIRIC:
                                res = new SiricBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.DGRSP:
                                res = new DgrspBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.BNI:
                                res = new BniBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.BNI_ATOS:
                                res = new BniAtosBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.FGADM:
                                res = new FGADMBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.SIATT:
                                res = new SIATTBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.E360:
                                res = new E360BusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.INVENTARIOSBusinessValidator:
                                res = new INVENTARIOSBusinessValidator();
                                break;
                            case RoundComb.Commons.Constants.BusinessValidators.RALBusinessValidator:
                                res = new RALBusinessValidator();
                                break;


                        }
                    }

                    return res;
                }

                private IBusinessCustomization GetBusinessCustomizationEntity(int idMsgType, string idMsgTypeEntity)
                {
                    IBusinessCustomization res = null;

                    TipoMensagemExternalModel tipoMensagemExternalModel = new DataAccess.DataAccess().getExternalMessageTypeByTypeAndEntity(idMsgType, idMsgTypeEntity);

                    if (tipoMensagemExternalModel != null && !string.IsNullOrEmpty(tipoMensagemExternalModel.msgBusinessValidator))
                    {
                        switch (tipoMensagemExternalModel.msgBusinessValidator)
                        {
                            case RoundComb.Commons.Constants.BusinessValidators.SIATT:
                                res = new SIATTBusinessCustomization();
                                break;

                        }
                    }

                    return res;
                }


                private IBusinessData GetBusinessEntity(string msgBody, int idMsgType, string idMsgTypeEntity)
                {
                    IBusinessData res = null;

                    TipoMensagemExternalModel tipoMensagemExternalModel = new DataAccess.DataAccess().getExternalMessageTypeByTypeAndEntity(idMsgType, idMsgTypeEntity);

                    if (tipoMensagemExternalModel != null && !string.IsNullOrEmpty(tipoMensagemExternalModel.msgSchema))
                    {
                        switch (tipoMensagemExternalModel.msgSchema)
                        {
                            case RoundComb.Commons.Constants.BusinessEntity.ProcessoExternalModel:
                                res = DeserializerExternalModels.DeserializeExternalProcesso(msgBody);
                                break;
                            case RoundComb.Commons.Constants.BusinessEntity.IgnoreXMLSchemaValidation:// nao ha validacao do xml, mas por defeito o BusinessEntity corresponde a ProcessoExternalModel
                                tipoMensagemExternalModel.msgSchema = RoundComb.Commons.Constants.BusinessEntity.ProcessoExternalModel;
                                res = DeserializerExternalModels.DeserializeExternalProcesso(msgBody);
                                break;
                            case RoundComb.Commons.Constants.BusinessEntity.MessageExternalModel:
                                res = DeserializerExternalModels.DeserializeExternalMessage(msgBody);
                                break;
                        }
                    }

                    return res;
                }

                private int getIdExternalEntity(string idMsgEntity)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    TipoMensagemExternalModel messageType = dataAccess.getTipoMensagemExternaList(idMsgEntity).FirstOrDefault();

                    return messageType != null ? messageType.idExternalEntity : 0;
                }

                public UnidadeOrganicaExternalModel getUnidadeOrganicaById(int idExternalEntity)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.getUnidadeOrganicaById(idExternalEntity);
                }

                private List<PropertyModel> proceedWithValidation(object obj, int idTribunal, string idMsgEntity)
                {
                    List<PropertyModel> processedPropList = new List<PropertyModel>();

                    PropertyInfo[] propertyInfoList = obj.GetType().GetProperties();

                    bool continueValidation = ContentValidation.isExpectedEntity(obj, idMsgEntity);

                    if (continueValidation && propertyInfoList != null && propertyInfoList.Length > 0)
                    {
                        foreach (PropertyInfo pinfo in propertyInfoList)
                        {
                            // Use this for real tests; for now this case is not reached anymore...
                            //if (propertyInfoList.Count() == 3 && pinfo.Name.Equals("Count"))
                            //    break;

                            object propertyValue = pinfo.GetValue(obj, null);

                            if (propertyValue != null && pinfo.PropertyType != null)
                            {
                                if ((pinfo.PropertyType != null && pinfo.PropertyType.IsPrimitive) || propertyValue.GetType() == typeof(string))
                                {
                                    LookForAttributes(idTribunal, idMsgEntity, pinfo, propertyValue, processedPropList);
                                }
                                else if (pinfo.PropertyType != typeof(DateTime?) && pinfo.PropertyType != typeof(string) && pinfo.PropertyType != typeof(decimal))
                                {
                                    List<object> objectList = ContentValidation.getPropertiesFromObject(propertyValue);

                                    foreach (object element in objectList)
                                    {
                                        List<PropertyModel> lst = proceedWithValidation(element, idTribunal, idMsgEntity);

                                        if (lst != null && lst.Count > 0)
                                            processedPropList.AddRange(lst);
                                    }
                                }
                            }
                        }
                    }

                    return processedPropList;
                }

                private void LookForAttributes(int idTribunal, string idMsgEntity, PropertyInfo pinfo, object propertyValue, List<PropertyModel> processedPropList)
                {
                    try
                    {
                        if (pinfo.IsDefined(typeof(ReferenciasGeraisMetadataAttribute), false))
                        {
                            int[] references = pinfo.GetCustomAttributes(typeof(ReferenciasGeraisMetadataAttribute), false).Cast<ReferenciasGeraisMetadataAttribute>().Single().Values;

                            List<int> topicsWithInvalidRefs = getInvalidReferences((int)propertyValue, references);
                            if (topicsWithInvalidRefs != null && topicsWithInvalidRefs.Count > 0)
                                processedPropList.Add(new PropertyModel() { Name = pinfo.Name, Value = propertyValue != null ? propertyValue.ToString() : null, TopicIds = string.Join(",", topicsWithInvalidRefs) });
                        }
                        else if (pinfo.IsDefined(typeof(TableMetadataAttribute), false))
                        {
                            string tableName = pinfo.GetCustomAttributes(typeof(TableMetadataAttribute), false).Cast<TableMetadataAttribute>().Single().TableName;
                            string propertyId = pinfo.GetCustomAttributes(typeof(TableMetadataAttribute), false).Cast<TableMetadataAttribute>().Single().PropertyId;

                            string tableWithInvalidRefs = getInvalidReferences(propertyValue, tableName, propertyId, idTribunal, idMsgEntity);

                            if (!string.IsNullOrEmpty(tableWithInvalidRefs))
                                processedPropList.Add(new PropertyModel() { Name = pinfo.Name, Value = propertyValue != null ? propertyValue.ToString() : null, TopicIds = tableWithInvalidRefs });
                        }
                    }
                    catch (Exception ex)
                    {
                        processedPropList.Add(new PropertyModel() { Name = pinfo.Name, Value = propertyValue != null ? propertyValue.ToString() : null, TopicIds = ex.Message });
                    }           
                }

                private string getInvalidReferences(object idReference, string tableName, string propertyId, int idTribunal, string idMsgEntity)
                {
                    string tableWithInvalidRefs = string.Empty;

                    int idRefInt = GenericMethods.ConvertToInt(idReference.ToString());

                    if (idRefInt != 0 || tableName.Equals(DataSources.Paises) || tableName.Equals(DataSources.Freguesias) || tableName.Equals(DataSources.EstabelecimentosPrisionais))
                    {
                        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                        switch (tableName)
                        {
                            case DataSources.Freguesias:
                                List<FreguesiaExternalModel> freguesias = dataAccess.getFreguesiasList();
                                tableWithInvalidRefs = ContentValidation.findItemByString<FreguesiaExternalModel>(freguesias, idReference, propertyId, tableName);
                                break;

                            case DataSources.EstabelecimentosPrisionais:
                                List<EstabelecimentosPrisionaisModel> prisoes = dataAccess.getEstabelecimentosPrisionaisList();
                                tableWithInvalidRefs = ContentValidation.findItemByString<EstabelecimentosPrisionaisModel>(prisoes, idReference, propertyId, tableName);
                                break;

                            case DataSources.MapeamentoAtos:
                                List<AtoProcessualMapeamentoExternalModel> atos = dataAccess.getAtosProcessuaisMapeamentoExternalList(idMsgEntity);
                                tableWithInvalidRefs = ContentValidation.findItemByInt<AtoProcessualMapeamentoExternalModel>(atos, idReference, propertyId, tableName);
                                break;

                            case DataSources.DesignacaoIntervenientes:
                                List<TipoIntervenienteExternalModel> tiposInterv = dataAccess.getTipoIntervenienteList();
                                tableWithInvalidRefs = ContentValidation.findItemByInt<TipoIntervenienteExternalModel>(tiposInterv, idReference, propertyId, tableName);
                                break;

                            case DataSources.PosicaoInterveniente:
                                List<PosicaoIntervenienteExternalModel> posicoesInterv = dataAccess.getPosicoesIntervenienteList();
                                tableWithInvalidRefs = ContentValidation.findItemByInt<PosicaoIntervenienteExternalModel>(posicoesInterv, idReference, propertyId, tableName);
                                break;

                            case DataSources.Crimes:
                                List<TipoCrimeExternalModel> crimes = dataAccess.getTiposCrimeList();
                                tableWithInvalidRefs = ContentValidation.findItemByInt<TipoCrimeExternalModel>(crimes, idReference, propertyId, tableName);
                                break;

                            case DataSources.TiposPessoa:
                                List<TipoPessoaExternalModel> pessoas = dataAccess.getTiposPessoaList();
                                tableWithInvalidRefs = ContentValidation.findItemByInt<TipoPessoaExternalModel>(pessoas, idReference, propertyId, tableName);
                                break;

                            case DataSources.TiposProcesso:
                                List<TipoProcessoExternalModel> tiposProcesso = dataAccess.getTiposProcessoList();
                                tableWithInvalidRefs = ContentValidation.findItemByInt<TipoProcessoExternalModel>(tiposProcesso, idReference, propertyId, tableName);
                                break;

                            case DataSources.Tribunais:
                                List<TribunalExternalModel> tribunais = dataAccess.getTribunaisList();
                                tableWithInvalidRefs = ContentValidation.findItemByInt<TribunalExternalModel>(tribunais, idReference, propertyId, tableName);
                                break;

                            case DataSources.UnidadesOrganicas:
                                List<UnidadeOrganicaExternalModel> unidadesOrganicas = dataAccess.getUnidadesOrganicasListByIdTribunal(idTribunal);
                                tableWithInvalidRefs = ContentValidation.findItemByInt<UnidadeOrganicaExternalModel>(unidadesOrganicas, idReference, propertyId, tableName);
                                break;

                            case DataSources.Paises:
                                List<PaisExternalModel> paises = dataAccess.getPaisesList();
                                tableWithInvalidRefs = ContentValidation.findItemByString<PaisExternalModel>(paises, idReference, propertyId, tableName);
                                break;
                        }
                    }

                    return tableWithInvalidRefs;
                }

                private List<int> getInvalidReferences(int idReference, int[] topicIds)
                {
                    List<int> topicsWithInvalidRefs = new List<int>();

                    if (idReference != 0)
                    {
                        foreach (int topicId in topicIds)
                        {
                            List<ReferenciaGeralExternalModel> tabela = new DataAccess.DataAccess().getTabelaReferenciaById(topicId);

                            if (tabela != null && tabela.Count > 0)
                            {
                                ReferenciaGeralExternalModel refGeral = tabela.Find(item => item.id == idReference);

                                if (refGeral == null)
                                    topicsWithInvalidRefs.Add(topicId);
                            }
                        }
                    }

                    return topicsWithInvalidRefs;
                }

                #endregion


                #region Message Queue Receive        

                #region GetDocs

                public List<MessageQueueManagerModel> MessageQueueReceiveGetDocsReg(int idTribRef)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<MessageQueueManagerModel> lista = new List<MessageQueueManagerModel>();
                    lista = dataAccess.MessageQueueReceiveGetDocsReg(idTribRef);
                    return lista;
                }

                public void MessageQueueReceiveSetReadyToProcess(int idQueue, string msgBodyWithDocs, string errormessage)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    dataAccess.MessageQueueReceiveSetReadyToProcess(idQueue, msgBodyWithDocs, errormessage);
                }

                #endregion GetDocs

                #region ProcessCreation


                public void createProcessesFromReceivedMessages()
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<ServidoresModel> servidores = dataAccess.CitiusNextGetServidoresLista();

                    foreach (ServidoresModel servidor in servidores)
                    {
                        Task.Factory.StartNew(() => createProcessesFromReceivedMessagesSingleTribunal(servidor.idTribRef));

                        //createProcessesFromReceivedMessagesSingleTribunal(servidor.idTribRef);
                    }
                }

                private void createProcessesFromReceivedMessagesSingleTribunal(int idTribRef)
                {
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    List<MessageQueueManagerModel> lstRegsToMigrate = new List<MessageQueueManagerModel>();
                    lstRegsToMigrate = dataAccess.MessageQueueReceiveGetReadyToProcess(idTribRef);
                    foreach (MessageQueueManagerModel message in lstRegsToMigrate)
                    {
                        if (IsMsgTypeAtivo(message.idMsgType))
                        {
                            try
                            {
                                MessageQueueManagerModel newMsg;

                                if (!message.isExcecao)
                                {
                                    newMsg = createProcessFromMessage(message);

                                }
                                else
                                {
                                    newMsg = updateWSReplyMessage(message);

                                }

                                dataAccess.MessageQueueReceiveSetDone(newMsg);

                            }
                            catch (Exception ex)
                            {
                                dataAccess.MessageQueueError(message.idQueue, -1, ex.Message);
                            }
                        }

                    }
                }

                private MessageQueueManagerModel createProcessFromMessage(MessageQueueManagerModel msg)
                {                        
                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    return dataAccess.createProcessFromMessage(msg);
                }        

                private MessageQueueManagerModel updateWSReplyMessage(MessageQueueManagerModel msg)
                {
                    TipoMensagemExternalModel tipoMensagemExternalModel = new DataAccess.DataAccess().getExternalMessageTypeByTypeAndEntity(msg.idMsgType, msg.idMsgSender);

                    if (tipoMensagemExternalModel != null && !string.IsNullOrEmpty(tipoMensagemExternalModel.msgExternalDataProvider))
                    {
                        switch (tipoMensagemExternalModel.msgExternalDataProvider)
                        {
                            case RoundComb.Commons.Constants.ExternalDataProviders.FGADM:
                                Fgadm fgdm = new Fgadm();
                                msg = fgdm.updateMessageWSResponse(msg.msgBody, msg);
                                break;

                        }

                    }


                    return msg;;
                }        


                #endregion ProcessCreation

                #endregion

            */
    }
}
