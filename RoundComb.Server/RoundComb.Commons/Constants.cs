using System.Collections.Generic;

namespace RoundComb.Commons
{
    public static class Constants
    {


        public static class Errors
        {
            public static class ErrorNumbers
            {
                public const int NO_ERROR = 0;
                public const int ERROR_GENERICO = -10000;
                public const int ERROR_CREATENEWEVENT= -1;
                public const int ERROR_GETLISTOFEVENTS = -2;
                public const int ERROR_GETLISTOFMYPROPERTIES = -3;
                public const int ERROR_GETSIGNURL = -4;
                public const int ERROR_SETUSERCLICK = -5;


                public const int READ_ACCESS_KEY = -10010;
            }

            public static class ErrorMessages
            {
                public const string ERROR_GENERICO = "An error ocurred. Please contact the system administrator";
                public const string READ_ACCESS_KEY = "An error ocurred to read the acess key.";
                public const string ERROR_CREATENEWEVENT = "An error ocurred to create the new event. We'll fix as soon as possible.";
                public const string ERROR_GETLISTOFEVENTS = "An error ocurred to get the list of events. We'll fix as soon as possible.";
                public const string ERROR_GETLISTOFMYPROPERTIES = "An error ocurred to get the list of my properties. We'll fix as soon as possible.";

            }
        }
        /*public static class Messages
        {
            public const string CREATENEWEVENT = "An error ocurred to create new event. Please contact the support.";
            /*public const string INFO_TECNICA_NOT_AVAILABLE = "O documento não tem informação técnica.";
            public const string DOC_RECEBIDO_INVALIDO = "O documento recebido é inválido.";
            public const string DOC_RECEBIDO_NAO_INSERIDO_NO_GESTOR_DOCUMENTAL = "O documento não foi inserido no gestor documental.";

            public const string NAO_RECEBIDO = "A mensagem não foi recebida.";
            public const string RECEBIDO_NAO_ENTREGUE_TRIBUNAL = "A mensagem foi recebida, mas ainda não foi entregue no tribunal";
            public const string RECEBIDO_ENTREGUE_TRIBUNAL = "A mensagem foi recebida e entregue no tribunal.";
            public const string ESTADO_COMUNICACAO_DESCONHECIDO = "Não foi possível obter o estado de comunicação do processo.";

            public const string DISTRIBUIDO = "O processo foi distribuido.";
            public const string NAO_DISTRIBUIDO = "O processo ainda não foi distribuido.";
            public const string ESTADO_DISTRIBUICAO_DESCONHECIDO = "Não foi possível obter o estado da distribuição do processo.";

            public const string TAMANHO_MAX_EXCEDIDO = "O tamanho do ficheiro excedeu o valor máximo";
            


        }
        
        public static class Namespaces
        {
            public const string INEROPERABILITY_TARGET_NAMESPACE = "http://schemas.datacontract.org/2004/07/CitiusNext.External.Interoperability.Models";
        }

        public static class Documentos
        {
            public const int MAX_FILE_SIZE_BYTES = 26214400;
        }

        public static class StringFormaters
        {
            public const string TRACE_REQUEST = "({0}) : {1}\r\nRequest:\r\n{2}";
            public const string TRACE_RESPONSE = "({0}) : {1}\r\nResponse:\r\n{2}";
            public const string DEBUG = "({0}) : {1} - {2}";
            public const string SEMICOLON = ";";
            public const string ERROR_VALUE_TO_CONCATENATE = " (valor identificado = {0})";
            public const string ERROR_MESSAGE_WITH_ERROR_VALUE = " ({0}) {1}";
            public const string WS_ERROR_MESSAGE_WITH_EXCEPTION = "[{0}] {1} - {2}";
            public const string WS_ERROR_MESSAGE_WITH_CLIENT_MESSAGE = "[{0}] {1} - Erro do cliente: {2} - {3}";
        }

        public static class Operations
        {
            public static class TiposLista
            {
                public const int MSG_EXTERNA_IGNORE_MSG_TYPE = 0;
                public const int MSG_EXTERNA_DO_NOT_IGNORE_MSG_TYPE = 1;
                public const int PAIS_BY_ISOPAIS = 1;
                public const int TRIBUNAIS_PARA_INTEROPERABILIDADE = 1;
                public const int TRIBUNAIS_PARA_INTEROPERABILIDADE_UNSMAPA_BASE = 2;
                public const int UNIDADES_ORGANICAS_PARA_INTEROPERABILIDADE = 2;
                public const int UNIDADES_ORGANICAS_PARA_INTEROPERABILIDADE_UNSMAPA_BASE = 4;
                public const int UNIDADE_ORGANICA_BY_ID = 1;
                public const int ATOS_MAP_EXTERNAL_PARA_INTEROPERABILIDADE = 1;
                public const int ATOS_SAIDA_MAP_EXTERNAL_PARA_INTEROPERABILIDADE = 2;
                public const int TIPO_INTERVENIENTE_PARA_INTEROPERABILIDADE = 1;
            }

            public static class Message_Queue_Manager_Operations
            {
                public const int INSERT_FROM_CLIENTE_CITIUS = 1;
                public const int INSERT_FROM_ENTIDADE_EXTERNA = 2;
                public const int UPDATE_DOCUMENTOS_MIGRADOS = 3;
                public const int UPDATE_PRONTO_ENVIAR = 4;
                public const int UPDATE_DONE = 5;
                public const int UPDATE_ERRO_FROM_ENTIDADE_EXTERNA = 6;
                public const int UPDATE_NOT_ATIVO = 7;
                public const int GET_NOT_MIGRATED = 8;
                public const int GET_DOCS_MIGRATED = 9;
                public const int GET_READY_TO_SEND = 10;
                public const int GET_DOCS_FROM_EXTERNAL	= 11;
			    public const int SET_READY_TO_PROCESS = 12;
                public const int GET_READY_TO_PROCESS = 13;
                public const int SET_PROCESS_PARTES = 14;
                public const int GET_ROW_BY_IDQUEUE = 15;
                public const int GET_ROW_BY_IDMSG = 16;
            }

            public static class External_Message_Entities_Process_Behaviours_Manager_Operations
            {
                public const int GET_ALL_ACTIVE = 1;
                public const int GET_BY_ID_SP = 2;
                public const int GET_BY_SPNAME = 3;
            }

            public static class Message_Get_Destination_Operations
            {
                public const int DEFAULT_GET = 1;
                public const int GET_DADOS_PAPEL_BY_IDMSG = 2;
            }

            public static class Message_Type_Operations
            {                
                public const int GET_MESSAGE_TYPE_SP = 1;
                public const int GET_MESSAGE_TYPE_IS_ATIVO = 2;
            }

            public static class External_Message_Get_Ato_IDs
            {
                public const int GET_ATO_MAPEADO = 0;
                public const int GET_TIPO_PAPEL = 1;
            }

            public static class API_WRITE_FGADM_COMARCA_UPDATE
            {
                public const int UPDATE_SUCESSO_FGADM = 3;
                public const int UPDATE_ERRO_FGADM = 4;
            }

        }

        

        public static class ExternalEntities
        {
            public static class ExternalCode
            {
                public const string SISAAE = "";
                public const string GNR = "4777CE87-BFFA-46C8-8409-1AFB974D8E54";   // Guarda Nacional Republicana
                public const string SIRIC = "B1AA8207-F2E1-4364-B829-FCA8F8BDFDE7"; // Sistema Integrado do Registo e Identificação Civil
                public const string DGRSP = "1446D4B0-09DF-4825-B412-E7505A3144F5"; // Direção-Geral de Reinserção e Serviços Prisionais
                public const string BNI = "DEB8FB7D-7939-42B1-9734-5BF4E65550EA";// Sistema Integrado com o Balcao Nacional de Injucoes
                public const string CITIUS = "4E25688C-023F-4DF7-AC6A-480231D3EBC7";// Citius
                public const string SEGSOCIALGERAL = "86159555-4134-4242-8C77-80599E2A7136";// Seguranca Social GERAL
                public const string SEGSOCIALFGADM = "0C5954FC-9155-4A9D-BD90-0F520D649B4C";// Seguranca Social FGADM
                public const string SEGSOCIALSIATT = "B89D83DB-7B6A-4857-AAE2-C2DD122F9567";// Seguranca Social SIATT
                public const string BDP = "563F13AA-8B1A-452D-8110-B35F93BC3080";// Banco de Portugal
                public const string E360 = "563F13AA-8B1A-452D-8110-B35F93BC3080";// Banco de Portugal
            }

            public static class ExternalidUnidadeOrganica
            {
                public const int SIRIC = 9000003; // Sistema Integrado do Registo e Identificação Civil
                public const int DGRSP = 9000005; // Direção-Geral de Reinserção e Serviços Prisionais
                public const int BNI = 9000006;// Sistema Integrado com o Balcao Nacional de Injucoes
                public const int SEGSOCIALFGADM = 9000008;// Seguranca Social FGADM
                public const int SEGSOCIALSIATT = 9000009;// Seguranca Social SIATT
                public const int BDP = 9000007;// Banco de Portugal
                public const int E360 = 9000010; // Escola 360
            }
        }

        public static class InternalEntities
        {
            public static class InternalCode
            {
                public const int CITIUS = 9100001;
            }

        }

        public static class EstadoComunicacaoProcesso
        {
            public const int NAO_RECEBIDO = 1;
            public const int RECEBIDO_NAO_ENTREGUE_TRIBUNAL = 2;
            public const int RECEBIDO_ENTREGUE_TRIBUNAL = 3;
            public const int ESTADO_COMUNICACAO_DESCONHECIDO = -1;
        }

        public static class EstadoDistribuicaoProcesso
        {
            public const int NAO_DISTRIBUIDO = 5;
            public const int DISTRIBUIDO = 4;
            public const int ESTADO_DISTRIBUICAO_DESCONHECIDO = -2;
        }

        public static class DocumentMetdata
        {
            public const int NUMBER_OF_ELEMENTS_ON_NAME = 5;
            public const int POSICAO_CHAVE_DOC = 1;
            public const int POSICAO_ID_SERVIDOR = 3;
            public const int POSICAO_ID_DOC_GESTOR = 4;
            public const int ID_UTIL_DEFAULT = 1;
            public const string UTILIZADOR_DEFAULT = "DEFAULT";
        }

        public static class BusinessEntity
        {
            public const string ProcessoExternalModel = "ProcessoExternalModel";
            public const string MessageExternalModel = "MessageExternalModel";
            public const string ProcessoExternalModelSimplified = "ProcessoExternalModelSimplified";
            public const string IgnoreXMLSchemaValidation = "IgnoreXMLSchemaValidation";
        }

        public static class GestorDocumentalExterno
        {
            public const string FILE_NET = "GestorDocumentalFileNet";
            public const string GESTOR_DOCUMENTAL_DGRSP = "GestorDocumentalDGRSP";
            public const string GESTOR_DOCUMENTAL_BNI = "GestorDocumentalBNI";
            public const string GESTOR_DOCUMENTAL_INVENTARIOS = "GestorDocumentalINVENTARIOS";
            public const string GESTOR_DOCUMENTAL_SIATT = "GestorDocumentalSIATT";
            public const string GESTOR_DOCUMENTAL_RAL = "GestorDocumentalRAL";
            public const string GESTOR_DOCUMENTAL_E360 = "GestorDocumentalE360";
        }

        public static class BusinessValidators
        {
            public const string SIRIC = "SiricBusinessValidator";
            public const string DGRSP = "DgrspBusinessValidator";
            public const string BNI = "BniBusinessValidator";
            public const string BNI_ATOS = "BniAtosBusinessValidator";
            public const string FGADM = "FGADMBusinessValidator";
            public const string SIATT = "SIATTBusinessValidator";
            public const string E360 = "E360BusinessValidator";
            public const string INVENTARIOSBusinessValidator = "INVENTARIOSBusinessValidator";
            public const string RALBusinessValidator = "RALBusinessValidator";
            
        }

        public static class FGDAM
        {
            public static class FGDAM_TIPO_OPERACAO
            {
                public const string NOVO = "N";
                public const string RENOVAR = "R";
                public const string CESSACAO = "C";
            }

            public static class FGDAM_METODOS
            {
                public const string NOVO = "registarPrestacao";
                public const string RENOVAR = "renovarManutencaoPrestacao";
                public const string CESSACAO = "renovarManutencaoPrestacao";
            }

        }

        public static class ExternalDataProviders
        {
            public const string SIRIC = "Siric";
            public const string DGRSP = "Dgrsp";
            public const string BNI = "Bni";
            public const string FGADM = "FGADM";
            public const string SIATT = "SIATT";
            public const string BDP = "BdP";
            public const string E360 = "E360";
            public const string PublicacaoPortal = "PublicacaoPortal";
            public const string Inventarios = "INVENTARIOS";
            public const string RAL = "RAL";
        }

        public static class WsProxy
        {
            public static class BindingType
            {
                public const int BASIC_BINDING = 1;
                public const int WS_HTTP_BINDING = 2;
            }
            public static class SecurityMode
            {
                public const int TRANSPORT = 1;
                public const int TRANSPORT_WITH_MESSAGE_CREDENCIAL = 2;
                public const int NONE = 3;
            }
            public static class AuthenticationMode
            {
                public const int NONE = 1;
                public const int BASIC = 2;
                public const int NTLM = 3;
                public const int USER_NAME = 4;
            }

            public static class MessageEncoding
            {
                public const string MTOM = "Mtom";
                public const string TEXT = "Text";
            }
        }

        public static class Settings
        {
            public static class AppKeys
            {
                public const string KEY = "key";
                public const string DOMAIN = "domain";
                public const string VALIDATE_CERTIFICATE_ON_REMOTE_WS = "validateCertificateOnRemoteWS";
            }

            public static class Bindings
            {
                public const string WS_HTTP_BINDING = "WSHttpBinding_Externals";
            }
        }

        #region Dictionary

        public static Dictionary<int, List<string>> getMethodsVersusClientDictionary()
        {
            Dictionary<int, List<string>> dicWsMethodsByClient = new Dictionary<int, List<string>>();
            List<string> wsMethods = null;

            #region GNR

            //wsMethods = new List<string>();
            //wsMethods.Add("isAlive");
            //wsMethods.Add("getMsgBodyXsd");
            //wsMethods.Add("getMessageCompleteXsd");
            //wsMethods.Add("GetTabelasReferencia");
            //wsMethods.Add("GetTabelaReferenciaById");
            //wsMethods.Add("GetTipoMensagemList");
            //wsMethods.Add("GetTipoProcessoList");
            //wsMethods.Add("getTribunaisList");
            //wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            //wsMethods.Add("GetPaisesList");
            //wsMethods.Add("GetFreguesiasList");
            //wsMethods.Add("GetCrimeEnquadramentoList");
            //wsMethods.Add("GetCrimeSubEnquadramentoList");
            //wsMethods.Add("GetTiposCrimeList");
            //wsMethods.Add("GetEstabelecimentosPrisionaisList");
            //wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            //wsMethods.Add("GetTiposIntervenienteList");
            //wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            //wsMethods.Add("GetTiposPessoaList");
            //wsMethods.Add("GetPosicoesIntervenienteList");
            //wsMethods.Add("GetMessageValidation");
            //wsMethods.Add("SetMessageSubmit");
            //wsMethods.Add("getDocumento");

            //dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.GNR, wsMethods);

            #endregion

            #region SISAAE

            //wsMethods = new List<string>();
            //wsMethods.Add("isAlive");
            //wsMethods.Add("getMsgBodyXsd");
            //wsMethods.Add("getMessageCompleteXsd");
            //wsMethods.Add("GetTabelasReferencia");
            //wsMethods.Add("GetTabelaReferenciaById");
            //wsMethods.Add("GetTipoMensagemList");
            //wsMethods.Add("GetTipoProcessoList");
            //wsMethods.Add("getTribunaisList");
            //wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            //wsMethods.Add("GetPaisesList");
            //wsMethods.Add("GetFreguesiasList");
            //wsMethods.Add("GetCrimeEnquadramentoList");
            //wsMethods.Add("GetCrimeSubEnquadramentoList");
            //wsMethods.Add("GetTiposCrimeList");
            //wsMethods.Add("GetEstabelecimentosPrisionaisList");
            //wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            //wsMethods.Add("GetTiposIntervenienteList");
            //wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            //wsMethods.Add("GetTiposPessoaList");
            //wsMethods.Add("GetPosicoesIntervenienteList");
            //wsMethods.Add("GetMessageValidation");
            //wsMethods.Add("SetMessageSubmit");
            //wsMethods.Add("getDocumento");

            //dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.SISAAE, wsMethods);

            #endregion

            #region SIRIC

            wsMethods = new List<string>();
            wsMethods.Add("isAlive");
            wsMethods.Add("getMsgBodyXsd");
            wsMethods.Add("getMessageCompleteXsd");
            wsMethods.Add("GetTabelasReferencia");
            wsMethods.Add("GetTabelaReferenciaById");
            wsMethods.Add("GetTipoMensagemList");
            wsMethods.Add("GetTipoProcessoList");
            wsMethods.Add("getTribunaisList");
            wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            wsMethods.Add("GetPaisesList");
            wsMethods.Add("GetFreguesiasList");
            wsMethods.Add("GetCrimeEnquadramentoList");
            wsMethods.Add("GetCrimeSubEnquadramentoList");
            wsMethods.Add("GetTiposCrimeList");
            wsMethods.Add("GetEstabelecimentosPrisionaisList");
            wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            wsMethods.Add("GetTiposIntervenienteList");
            wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            wsMethods.Add("GetTiposPessoaList");
            wsMethods.Add("GetPosicoesIntervenienteList");
            wsMethods.Add("getMessageValidation");
            wsMethods.Add("setMessageSubmit");
            wsMethods.Add("getDocumento");

            dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.SIRIC, wsMethods);

            #endregion

            #region DGRSP

            wsMethods = new List<string>();
            wsMethods.Add("isAlive");
            wsMethods.Add("getMsgBodyXsd");
            wsMethods.Add("getMessageCompleteXsd");
            wsMethods.Add("GetTabelasReferencia");
            wsMethods.Add("GetTabelaReferenciaById");
            wsMethods.Add("GetTipoMensagemList");
            wsMethods.Add("GetTipoProcessoList");
            wsMethods.Add("getTribunaisList");
            wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            wsMethods.Add("GetPaisesList");
            wsMethods.Add("GetFreguesiasList");
            wsMethods.Add("GetCrimeEnquadramentoList");
            wsMethods.Add("GetCrimeSubEnquadramentoList");
            wsMethods.Add("GetTiposCrimeList");
            wsMethods.Add("GetEstabelecimentosPrisionaisList");
            wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            wsMethods.Add("GetTiposIntervenienteList");
            wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            wsMethods.Add("GetTiposPessoaList");
            wsMethods.Add("GetPosicoesIntervenienteList");
            wsMethods.Add("getMessageValidation");
            wsMethods.Add("setMessageSubmit");
            wsMethods.Add("getDocumento");

            dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.DGRSP, wsMethods);

            #endregion

            #region BNI

            wsMethods = new List<string>();
            wsMethods.Add("isAlive");
            wsMethods.Add("getMsgBodyXsd");
            wsMethods.Add("getMessageCompleteXsd");
            wsMethods.Add("GetTabelasReferencia");
            wsMethods.Add("GetTabelaReferenciaById");
            wsMethods.Add("GetTipoMensagemList");
            wsMethods.Add("GetTipoProcessoList");
            wsMethods.Add("getTribunaisList");
            wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            wsMethods.Add("GetPaisesList");
            wsMethods.Add("GetFreguesiasList");
            wsMethods.Add("GetCrimeEnquadramentoList");
            wsMethods.Add("GetCrimeSubEnquadramentoList");
            wsMethods.Add("GetTiposCrimeList");
            wsMethods.Add("GetEstabelecimentosPrisionaisList");
            wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            wsMethods.Add("GetTiposIntervenienteList");
            wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            wsMethods.Add("GetTiposPessoaList");
            wsMethods.Add("GetPosicoesIntervenienteList");
            wsMethods.Add("getMessageValidation");
            wsMethods.Add("setMessageSubmit");
            wsMethods.Add("getDocumento");
            wsMethods.Add("getEstadoComunicacaoProcesso");
            wsMethods.Add("getEstadoDistribuicaoProcesso");

            dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.BNI, wsMethods);

            #endregion

            #region SEG SOCIAL FGADM

            wsMethods = new List<string>();
            wsMethods.Add("isAlive");
            wsMethods.Add("getMsgBodyXsd");
            wsMethods.Add("getMessageCompleteXsd");
            wsMethods.Add("GetTabelasReferencia");
            wsMethods.Add("GetTabelaReferenciaById");
            wsMethods.Add("GetTipoMensagemList");
            wsMethods.Add("GetTipoProcessoList");
            wsMethods.Add("getTribunaisList");
            wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            wsMethods.Add("GetPaisesList");
            wsMethods.Add("GetFreguesiasList");
            wsMethods.Add("GetCrimeEnquadramentoList");
            wsMethods.Add("GetCrimeSubEnquadramentoList");
            wsMethods.Add("GetTiposCrimeList");
            wsMethods.Add("GetEstabelecimentosPrisionaisList");
            wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            wsMethods.Add("GetTiposIntervenienteList");
            wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            wsMethods.Add("GetTiposPessoaList");
            wsMethods.Add("GetPosicoesIntervenienteList");
            wsMethods.Add("getMessageValidation");
            wsMethods.Add("setMessageSubmit");
            wsMethods.Add("getDocumento");

            dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.SEGSOCIALFGADM, wsMethods);

            #endregion

            #region SEG SOCIAL SIATT

            wsMethods = new List<string>();
            wsMethods.Add("isAlive");
            wsMethods.Add("getMsgBodyXsd");
            wsMethods.Add("getMessageCompleteXsd");
            wsMethods.Add("GetTabelasReferencia");
            wsMethods.Add("GetTabelaReferenciaById");
            wsMethods.Add("GetTipoMensagemList");
            wsMethods.Add("GetTipoProcessoList");
            wsMethods.Add("getTribunaisList");
            wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            wsMethods.Add("GetPaisesList");
            wsMethods.Add("GetFreguesiasList");
            wsMethods.Add("GetCrimeEnquadramentoList");
            wsMethods.Add("GetCrimeSubEnquadramentoList");
            wsMethods.Add("GetTiposCrimeList");
            wsMethods.Add("GetEstabelecimentosPrisionaisList");
            wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            wsMethods.Add("GetTiposIntervenienteList");
            wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            wsMethods.Add("GetTiposPessoaList");
            wsMethods.Add("GetPosicoesIntervenienteList");
            wsMethods.Add("getMessageValidation");
            wsMethods.Add("setMessageSubmit");
            wsMethods.Add("getDocumento");

            dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.SEGSOCIALSIATT, wsMethods);

            #endregion


            #region E360 Escola 360

            wsMethods = new List<string>();
            wsMethods.Add("isAlive");
            wsMethods.Add("getMsgBodyXsd");
            wsMethods.Add("getMessageCompleteXsd");
            wsMethods.Add("GetTabelasReferencia");
            wsMethods.Add("GetTabelaReferenciaById");
            wsMethods.Add("GetTipoMensagemList");
            wsMethods.Add("GetTipoProcessoList");
            wsMethods.Add("getTribunaisList");
            wsMethods.Add("getUnidadesOrganicasListByIdTribunal");
            wsMethods.Add("GetPaisesList");
            wsMethods.Add("GetFreguesiasList");
            wsMethods.Add("GetCrimeEnquadramentoList");
            wsMethods.Add("GetCrimeSubEnquadramentoList");
            wsMethods.Add("GetTiposCrimeList");
            wsMethods.Add("GetEstabelecimentosPrisionaisList");
            wsMethods.Add("GetAtosProcessuaisMapeamentoList");
            wsMethods.Add("GetTiposIntervenienteList");
            wsMethods.Add("GetTiposIntervenientePorTipoProcessoList");
            wsMethods.Add("GetTiposPessoaList");
            wsMethods.Add("GetPosicoesIntervenienteList");
            wsMethods.Add("getMessageValidation");
            wsMethods.Add("setMessageSubmit");
            wsMethods.Add("getDocumento");

            dicWsMethodsByClient.Add(Constants.ExternalEntities.ExternalidUnidadeOrganica.SEGSOCIALSIATT, wsMethods);

            #endregion

            return dicWsMethodsByClient;
        }
        #endregion

        */
    }
}
