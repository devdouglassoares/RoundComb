using RoundComb.Commons;
using System.Transactions;
using System;
using System.Data;
using RoundComb.Commons.Models;
using RoundComb.Commons.Dapper;
using RoundComb.Commons.ExceptionHandling;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace RoundComb.DataAccess
{
    public class DataAccess
    {

        #region Commons

        private TransactionOptions getTransactionOptions()
        {
            return new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.MaxValue };
        }

        public void checkParamsError(DynamicParameters parameters)
        {
            int response = Constants.Errors.ErrorNumbers.NO_ERROR;
            string error = "";
            try
            {
                error = parameters.Get<string>("@MsgErro") == null ? "" : parameters.Get<string>("@MsgErro");
                response = parameters.Get<int>("@Resp");
            }
            catch (Exception ex)
            {
                throw new Exception(Helper.msgToThrow(error, ex.Message));
            }

            if (response != 0 || !String.IsNullOrEmpty(error.Trim()))
            {
                throw new Exception(Helper.msgToThrow(error, ""));
            }
        }

        public IDbConnection getConnection()
        {
            return null;
        }

        #endregion

        #region chat room

        public RespostaContract<string> createNewChatRoom(string JSONObject)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.createNewChatRoom(JSONObject);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCreateNewEvent, parms, commandType: CommandType.StoredProcedure).ToString();
                    checkParamsError(parms);

                    string retGUID= parms.Get<string>("@chatroomguid");
                    Guid guidoutput;

                    if (Guid.TryParse(retGUID, out guidoutput))
                        ret.entidade = guidoutput.ToString();

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;
         
        }

       
        #endregion chat room

        #region events

        public RespostaContract<List<ListOfEventsModel>> GetMyListOfEvents(int iduser)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.GetMyListOfEvents(iduser);
            RespostaContract<List<ListOfEventsModel>> ret = new RespostaContract<List<ListOfEventsModel>>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret.entidade = SqlMapper.Query<ListOfEventsModel>(connection, SPsProvider.RoundCombGetListOfEvents, parms, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList<ListOfEventsModel>();
                    checkParamsError(parms);

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public RespostaContract<List<ListOfEventsCountModel>> GetMyListOfEventsCount(int iduser)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.GetMyListOfEventsCount(iduser);
            RespostaContract<List<ListOfEventsCountModel>> ret = new RespostaContract<List<ListOfEventsCountModel>>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret.entidade = SqlMapper.Query<ListOfEventsCountModel>(connection, SPsProvider.RoundCombGetListOfEventsCount, parms, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList<ListOfEventsCountModel>();
                    checkParamsError(parms);

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public RespostaContract<string> setUnreadMessage(UnreadMessageParam setmsg)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.setUnreadMessage(setmsg.idchatroom, setmsg.userid);

            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombSetReadMessages, parms, commandType: CommandType.StoredProcedure).ToString();
                    checkParamsError(parms);

                    ret.entidade = "done";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public RespostaContract<string> getCountUnReadMessages(string userid)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getCountUnReadMessages(userid);

            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret.entidade = connection.ExecuteScalar(SPsProvider.RoundCombGetUnreadMessages, parms, commandType: CommandType.StoredProcedure).ToString();
                    //checkParamsError(parms);

                    //var x = parms.Get<string>("@totalunreadmessages");
                    //ret.entidade = "59";


                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.msgErro = ex.Message;
            }

            return ret;

        }

        #endregion events



        #region Signature

        public RespostaContract<string> CheckSignatureExists(string Guid, string mysignerrole)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.CheckSignatureExists(Guid, mysignerrole);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCheckSignatureExists, parms, commandType: CommandType.StoredProcedure).ToString();
                    //checkParamsError(parms);

                    ret.entidade = parms.Get<string>("@Resp");

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;
        }

        public RespostaContract<string> CreateNewSignature(string chatroomguid, string Subject, string message,string SignatureRequestId, string signerRole_A, DateTime signerExpireDate_A, string signerSignatureId_A, string signerRole_B, DateTime signerExpireDate_B, string signerSignatureId_B)    
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.CreateNewSignature(chatroomguid, Subject, message, SignatureRequestId, signerRole_A, signerExpireDate_A, signerSignatureId_A, signerRole_B, signerExpireDate_B, signerSignatureId_B);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCreateUpdateSignature, parms, commandType: CommandType.StoredProcedure).ToString();

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public RespostaContract<string> setSignatureDocumentDone(string guidchatroom, string signerRole)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.setSignatureDocumentDone(guidchatroom, signerRole);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCreateUpdateSignature, parms, commandType: CommandType.StoredProcedure).ToString();

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public string getTemplateId(string guidchatroom)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getTemplateId(guidchatroom);
            string ret = "";

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret = connection.Execute(SPsProvider.RoundCombGetSignatureTemplateID, parms, commandType: CommandType.StoredProcedure).ToString();

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public RespostaContract<string> setHelloSignTemplateToNewProperty(string editurl, int propertyID)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.setHelloSignTemplateToNewProperty(editurl, propertyID);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCreateTemplateToNewProperty, parms, commandType: CommandType.StoredProcedure).ToString();

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        public RespostaContract<string> getSignatureRequestId(string guidchatroom)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getSignatureRequestId(guidchatroom);
     
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombGetSignatureRequestId, parms, commandType: CommandType.StoredProcedure).ToString();
                    ret.entidade = parms.Get<string>("@Resp");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);
            }

            return ret;

        }

        #endregion Signature

        #region global


        public RespostaContract<string> setUserClick(UserclickInfo userclick)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.setUserClick(userclick.deviceinfo, userclick.clickproperty);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombSetUserClick, parms, commandType: CommandType.StoredProcedure).ToString();
             
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }

        public RespostaContract<string> createNewContractDocument(ContractDocument contract)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.createNewContractDocument(contract.propertyID, contract.IdEvent, contract.name, contract.HtmlContent);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCreateNewContractDocument, parms, commandType: CommandType.StoredProcedure).ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }
        


        public RespostaContract<List<UserClickViewsPerProperty>> getUserClickViewPropertyDetails(string listofproperties)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getUserClickViewPropertyDetails(listofproperties);
            RespostaContract<List<UserClickViewsPerProperty>> ret = new RespostaContract<List<UserClickViewsPerProperty>>();
            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret.entidade = SqlMapper.Query<UserClickViewsPerProperty>(connection, SPsProvider.RoundCombGetUserViewsClickCount, parms, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList<UserClickViewsPerProperty>();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }

        public RespostaContract<List<ListOfMyProperties>> getMyProperties(string ownerid)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getMyProperties(ownerid);
            RespostaContract<List<ListOfMyProperties>> ret = new RespostaContract<List<ListOfMyProperties>>();
            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret.entidade = SqlMapper.Query<ListOfMyProperties>(connection, SPsProvider.RoundCombGetMyProperties, parms, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList<ListOfMyProperties>();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }

        public RespostaContract<List<ContractTemplateDocument>> getContractTemplatesDocuments(string eventid)
        {
            
            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getContractTemplatesDocuments(eventid);
            RespostaContract<List<ContractTemplateDocument>> ret = new RespostaContract<List<ContractTemplateDocument>>();
            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    ret.entidade = SqlMapper.Query<ContractTemplateDocument>(connection, SPsProvider.RoundCombContractTemplateDocument, parms, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList<ContractTemplateDocument>();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }

        public RespostaContract<string> getContractByPropertyId(string propertyId, string eventId)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getContractByPropertyId(propertyId, eventId);
            RespostaContract<string> ret = new RespostaContract<string>();
            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {

                    //ret.entidade = SqlMapper.Query <string>(connection,SPsProvider.RoundCombGetContractbyPropertyId, parms, commandType: CommandType.StoredProcedure).ToString();
                    ret.entidade = connection.ExecuteScalar(SPsProvider.RoundCombGetContractbyPropertyId, parms, commandType: CommandType.StoredProcedure).ToString();
                    /* checkParamsError(parms);

                     ret.entidade = parms.Get<string>("@htmlcontent");*/
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }

        public RespostaContract<string> getContractReplacedByPropertyId(string propertyId, string eventId, string HTMLContent)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.getContractReplacedByPropertyId(propertyId, eventId, HTMLContent);
            RespostaContract<string> ret = new RespostaContract<string>();
            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {

                    //ret.entidade = SqlMapper.Query <string>(connection,SPsProvider.RoundCombGetContractbyPropertyId, parms, commandType: CommandType.StoredProcedure).ToString();
                    ret.entidade = connection.ExecuteScalar(SPsProvider.RoundCombGetContractReplacedbyPropertyId, parms, commandType: CommandType.StoredProcedure).ToString();
                    /* checkParamsError(parms);

                     ret.entidade = parms.Get<string>("@htmlcontent");*/
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }
        

        public RespostaContract<string> setPropertyContractTemplate(string xmlParams, string TemplateId, string EditUrl, string ExpiresAt)
        {

            IDbConnection connection = null;
            DynamicParameters parms = ParametersProvider.setPropertyContractTemplate(xmlParams, TemplateId, EditUrl, ExpiresAt);
            RespostaContract<string> ret = new RespostaContract<string>();

            try
            {
                using (connection = ConnectionProvider.OpenConnectionDBServer())
                {
                    connection.Execute(SPsProvider.RoundCombCreateNewHelloSignDocument, parms, commandType: CommandType.StoredProcedure).ToString();

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleException(typeof(string), ex);

                ret.idErro = -1;
                ret.msgErro = ex.Message;
            }

            return ret;

        }

        #endregion global

    }
}
