using System.Collections.Generic;
using System.ServiceModel;

namespace RoundComb.ServiceLayer.SoapApi
{
    [ServiceContract]
    public interface ISoapApi
    {
        /*
        [OperationContract]
        RespostaContract<DocumentoContract> getDocumento(string idMsgEntity, string fileName);

        [OperationContract]
        RespostaContract<MessageExternalIdentificadorContract> getMessageValidation(string idMsgEntity, MessageExternalContract msg);

        [OperationContract]
        RespostaContract<MessageExternalIdentificadorContract> setMessageSubmit(string idMsgEntity, MessageExternalContract msg);

        [OperationContract]
        RespostaContract<SimpleMessageContract> isAlive(string idMsgEntity);

        [OperationContract]
        RespostaContract<SimpleMessageContract> getMessageCompleteXsd(string idMsgEntity);

        [OperationContract]
        RespostaContract<SimpleMessageContract> getMsgBodyXsd(string idMsgEntity, int msgTypeId);
        [OperationContract]
        RespostaContract<SimpleMessageContract> getMsgBodySimplifiedXsd(string idMsgEntity, int msgTypeId);

        [OperationContract]
        RespostaContract<List<TribunalExternalContract>> getTribunaisList(string idMsgEntity);
        [OperationContract]
        RespostaContract<List<UnidadeOrganicaExternalContract>> getUnidadesOrganicasListByIdTribunal(string idMsgEntity, int idTribunal);
        [OperationContract]
        RespostaContract<string> getEstadoDistribuicaoProcesso(string idMsgEntity, string processo);
        [OperationContract]
        RespostaContract<string> getEstadoComunicacaoProcesso(string idMsgEntity, string idMsg);

        [OperationContract]
        RespostaContract<string> getLocalizarProcesso(string idMsgEntity, string processo, string guidprocesso, string nifInterv);
        */
    }
}
