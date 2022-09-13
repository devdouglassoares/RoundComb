using System;
using System.Data;
using RoundComb.Commons.Dapper;

namespace RoundComb.DataAccess
{
    internal class ParametersProvider
    {
       
        #region Commons

        internal static DynamicParameters CommonOutputParams()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@MsgErro", "", dbType: DbType.String, direction: ParameterDirection.Output, size: 5000);
            param.Add("@Resp", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            return param;
        }

        #endregion

        #region chat room

        internal static DynamicParameters createNewChatRoom(string JSONObject)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters = CommonOutputParams();
            parameters.Add("@JSONContent", JSONObject, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@chatroomguid", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            
            return parameters;
        }
        #endregion chat room

        #region events

        internal static DynamicParameters GetMyListOfEvents(int iduser)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters = CommonOutputParams();
            parameters.Add("@myIDUser", iduser, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return parameters;
        }

        internal static DynamicParameters GetMyListOfEventsCount(int iduser)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters = CommonOutputParams();
            parameters.Add("@myIDUser", iduser, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return parameters;
        }


        internal static DynamicParameters setUnreadMessage(string idchatroom, string userid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters = CommonOutputParams();
            parameters.Add("@idchatroom", idchatroom, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@userid", userid, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return parameters;
        }

        internal static DynamicParameters getCountUnReadMessages(string userid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters = CommonOutputParams();
            parameters.Add("@userid", userid, dbType: DbType.Int32, direction: ParameterDirection.Input);
            //parameters.Add("@totalunreadmessages", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);
            
            return parameters;
        }

        


        #endregion events

        #region Signature

        internal static DynamicParameters CheckSignatureExists(string GUID,string mysignerrole)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GUIDChatroom", GUID, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@SignerRole", mysignerrole, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@Resp", "", dbType: DbType.String, direction: ParameterDirection.Output);


            return parameters;
        }

        internal static DynamicParameters CreateNewSignature(string chatroomguid, string Subject, string message, string SignatureRequestId, string signerRole_A, DateTime signerExpireDate_A, string signerSignatureId_A, string signerRole_B, DateTime signerExpireDate_B, string signerSignatureId_B)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GUIDChatroom", chatroomguid, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@Subject", Subject, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@message", message, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@SignatureRequestId", SignatureRequestId, dbType: DbType.String, direction: ParameterDirection.Input);

            parameters.Add("@signerRole_A", signerRole_A, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@signerExpireDate_A", signerExpireDate_A, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@signerSignatureId_A", signerSignatureId_A, dbType: DbType.String, direction: ParameterDirection.Input);

            parameters.Add("@signerRole_B", signerRole_B, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@signerExpireDate_B", signerExpireDate_B, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@signerSignatureId_B", signerSignatureId_B, dbType: DbType.String, direction: ParameterDirection.Input);


            return parameters;
        }

        internal static DynamicParameters setSignatureDocumentDone(string guidchatroom, string signerRole)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GUIDChatroom", guidchatroom, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@signerRole_A", signerRole, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@OperationType", "U", dbType: DbType.String, direction: ParameterDirection.Input);
            

            return parameters;
        }

        internal static DynamicParameters getTemplateId(string guidchatroom)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GUIDChatroom", guidchatroom, dbType: DbType.String, direction: ParameterDirection.Input);

            return parameters;
        }

        

        internal static DynamicParameters setHelloSignTemplateToNewProperty(string editurl, int propertyID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@editurl", editurl, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@propertyID", editurl, dbType: DbType.Int16, direction: ParameterDirection.Input);
            
            return parameters;
        }
        

        internal static DynamicParameters getSignatureRequestId(string guidchatroom)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GUIDChatroom", guidchatroom, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@Resp", "", dbType: DbType.String, direction: ParameterDirection.Output);

            return parameters;
        }

        #endregion Signature

        #region global

        internal static DynamicParameters setUserClick(string deviceinfo,string clickproperty)
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("@deviceinfo", deviceinfo, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@clickproperty", clickproperty, dbType: DbType.String, direction: ParameterDirection.Input);

            return parameters;
        }

        internal static DynamicParameters createNewContractDocument(int IdProperty, int IdEvent, string name, string HtmlContent)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Name", name, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ContractHTMLContent", HtmlContent, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@IDEvent", IdEvent, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@IDProperty", IdProperty, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return parameters;
        }

        

        internal static DynamicParameters getUserClickViewPropertyDetails(string listofproperties)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@listpropertyId", listofproperties, dbType: DbType.String, direction: ParameterDirection.Input);

            return parameters;
        }

        internal static DynamicParameters getMyProperties(string ownerid)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ownerid", ownerid, dbType: DbType.String, direction: ParameterDirection.Input);

            return parameters;
        }

        internal static DynamicParameters getContractTemplatesDocuments(string eventid)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@IDEvent", eventid, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return parameters;
        }

        internal static DynamicParameters getContractByPropertyId(string propertyId,string eventId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@propertyId", propertyId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@EventId", eventId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@htmlcontent", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);
            
            return parameters;
        }

        internal static DynamicParameters getContractReplacedByPropertyId(string propertyId, string eventId, string HTMLContent)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@propertyId", propertyId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@EventId", eventId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@htmlcontentwithvariables", HTMLContent, dbType: DbType.String, direction: ParameterDirection.Input);
            
            parameters.Add("@htmlcontent", "", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            return parameters;
        }
        



        internal static DynamicParameters setPropertyContractTemplate(string xmlParams, string TemplateId, string EditUrl, string ExpiresAt)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@xmlParams", xmlParams, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@TemplateId", TemplateId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@EditUrl", EditUrl, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ExpiresAt", ExpiresAt, dbType: DbType.String, direction: ParameterDirection.Input);

            return parameters;
        }

 
         #endregion global



    }
}
