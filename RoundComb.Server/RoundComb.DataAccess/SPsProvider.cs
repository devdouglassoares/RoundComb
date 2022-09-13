
namespace RoundComb.DataAccess
{
    internal class SPsProvider
    {
        #region chat room

        internal static string RoundCombCreateNewEvent { get { return "dbo.spc_createChatRoom"; } }

        #endregion chat room

        #region events

        internal static string RoundCombGetListOfEvents { get { return "dbo.spc_GetMyEventsLList"; } }
        internal static string RoundCombGetListOfEventsCount { get { return "dbo.spc_GetMyEventsLListCount"; } }
        internal static string RoundCombSetReadMessages { get { return "dbo.spc_SetReadMessages"; } }
        internal static string RoundCombGetUnreadMessages { get { return "dbo.spc_GetUnreadMessages"; } }
        #endregion events

        #region Signature

        internal static string RoundCombCheckSignatureExists { get { return "dbo.spc_CheckSignatureExists"; } }
        internal static string RoundCombCreateUpdateSignature { get { return "dbo.spc_CreateUpdateDocumentSignatureId"; } }
        internal static string RoundCombCreateTemplateToNewProperty { get { return "dbo.spc_CreateTemplateToNewProperty"; } }
        internal static string RoundCombGetSignatureRequestId { get { return "dbo.spc_getSignatureRequestId"; } }
        internal static string RoundCombGetSignatureTemplateID { get { return "dbo.spc_getSignatureTemplateId"; } }

        #endregion Signature

        #region global
        internal static string RoundCombCreateNewContractDocument { get { return "dbo.spc_CreateNewContractDocument"; } }
        internal static string RoundCombSetUserClick { get { return "dbo.spc_SetUserClick"; } }
        internal static string RoundCombGetUserViewsClickCount { get { return "dbo.spc_GetUserViewsClickCount"; } }
        internal static string RoundCombGetMyProperties { get { return "dbo.spc_GetMyProperties"; } }
        internal static string RoundCombCreateNewHelloSignDocument { get { return "dbo.spc_CreateNewHelloSignDocument"; } }
        internal static string RoundCombContractTemplateDocument { get { return "dbo.spc_GetContractTemplateDocuments"; } }
        internal static string RoundCombGetContractbyPropertyId { get { return "dbo.spc_GetContractbyPropertyId"; } }
        internal static string RoundCombGetContractReplacedbyPropertyId { get { return "dbo.spc_GetContractReplacedbyPropertyId"; } }

        #endregion global

    }
}
