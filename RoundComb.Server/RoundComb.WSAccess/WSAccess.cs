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
using System.Net;
using HelloSign;
using RoundComb.DataAccess;
using System.IO;

namespace RoundComb.WSAccess
{
    public class WSAccess
    {
        // queimado por agora mas tem que passar para o web.config
        string APIKEY = "b56d04089b5650ccb7508f7d6cd0432701fbf769f4f5ce614af147587a418692";
        string CLIENTID = "c1d837a4972d309967a81cccae7cb5c2";
        string FilesLocation = "C:\\inetpub\\Applications\\roundcomb\\upload.api\\Media\\roundcomb.rwi-cloud.com\\";

        Client client;
        DataAccess.DataAccess dataAccess;

       public WSAccess()
        {
            client = new Client(APIKEY);
            dataAccess = new DataAccess.DataAccess();

      
        }

        public RespostaContract<string> getSignUrl(SignatureRequestModel signaturerequest)
        {
            RespostaContract<string> resposta = new RespostaContract<string>();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string signatureid = dataAccess.CheckSignatureExists(signaturerequest.guidchatroom.ToString(), signaturerequest.mysignerRole).entidade;
                
                if (string.IsNullOrEmpty(signatureid))
                    signatureid = getSignatureId(signaturerequest.firstsignerRole, signaturerequest.firstsignerEmail, signaturerequest.firstsignerName, signaturerequest.secondsignerRole, signaturerequest.secondsignerEmail, signaturerequest.secondsignerName, signaturerequest.eventname, signaturerequest.Subject, signaturerequest.message, signaturerequest.guidchatroom.ToString(), signaturerequest.mysignerRole, signaturerequest.templateId);

                var sign = client.GetSignUrl(signatureid);

                resposta.entidade = sign.SignUrl;

            }
            catch(Exception ex)
            {
                resposta.idErro = -1;
                resposta.msgErro = ex.Message;
            }

            return resposta;
        }

        private string getSignatureId(string firstsignerRole, string firstsignerEmail, string firstsignerName, string secondsignerRole, string secondsignerEmail, string secondsignerName, string eventname, string Subject, string message, string guidchatroom,string myrole, string templateId)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var request = new TemplateSignatureRequest();


            request.AddTemplate(templateId);
            request.Subject = Subject;
            request.Message = message;

            request.AddSigner(firstsignerRole, firstsignerEmail, firstsignerName);
            request.AddSigner(secondsignerRole, secondsignerEmail, secondsignerName);

            request.SigningOptions = new SigningOptions
            {
                Draw = true,
                Type = true,
                Upload = true,
                Phone = false,
                Default = "draw"
            };
            request.TestMode = true;
            

            var response = client.CreateEmbeddedSignatureRequest(request, CLIENTID);

            DateTime datenow = DateTime.Now;

            dataAccess.CreateNewSignature(guidchatroom, Subject, message, response.SignatureRequestId, response.Signatures[0].SignerRole, datenow, response.Signatures[0].SignatureId,response.Signatures[1].SignerRole, datenow, response.Signatures[1].SignatureId);

            if(response.Signatures[0].SignerRole == myrole)
                return response.Signatures[0].SignatureId;
            else
                return response.Signatures[1].SignatureId;

        }
        /*
        private string getTemplateId(string guidchatroom)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

            return dataAccess.getTemplateId(guidchatroom);

            /*
            if (string.Compare(eventname, "Lease") == 0)
            {
                return "27608ca42e0d587852eb97adc0ac2e8cb3774df2"; //queimado por agora
            }
            else
                return "";
            */
        /*}
        */

        public RespostaContract<string> DownloadSignatureRequestFile(string signatureRequestId)
        {
            RespostaContract<string> resp = new RespostaContract<string>();

             try
             {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                resp.entidade = Convert.ToBase64String(client.DownloadSignatureRequestFiles(signatureRequestId, SignatureRequest.FileType.PDF));
             }
            catch (Exception ex)
            {
                resp.msgErro = ex.InnerException.Message;
            }

            return resp;
        }

        public RespostaContract<string> setPropertyContractTemplate(PropertyContractTemplate contractTemplateparams)
        {
            RespostaContract<string> resp = new RespostaContract<string>();

            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                
                FileContainer file = new FileContainer();
                
                string filename = string.Concat(FilesLocation,contractTemplateparams.fileURL);

                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    // Create a byte array of file stream length
                    byte[] bytes = System.IO.File.ReadAllBytes(filename);
                    //Read block of bytes from stream into the byte array
                    fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                    //Close the File Stream
                    fs.Close();
                    file.Bytes = bytes;
                }

                var draft = new EmbeddedTemplateDraft();
                draft.TestMode = true;
                draft.AddFile(file.Bytes, Path.GetFileName(filename));
                draft.Title = contractTemplateparams.Title;
                draft.Subject = contractTemplateparams.Subject;
                draft.Message = contractTemplateparams.Message;
                draft.AddSignerRole("Tenant", 0);
                draft.AddSignerRole("Landlord", 1);

                EmbeddedTemplate embeddedTemplate = new EmbeddedTemplate();

                embeddedTemplate = client.CreateEmbeddedTemplateDraft(draft, CLIENTID);

                if(embeddedTemplate.TemplateId != null && !String.IsNullOrEmpty(embeddedTemplate.TemplateId))
                {
                    string xmlParams = "<NewContractTemplate><fileURL>[&fileURL]</fileURL><Title>[&Title]</Title><Subject>[&Subject]</Subject><Message>[&Message]</Message></NewContractTemplate>";

                    xmlParams = xmlParams.Replace("[&fileURL]", contractTemplateparams.fileURL).Replace("[&Title]", contractTemplateparams.Title).Replace("[&Subject]", contractTemplateparams.Subject).Replace("[&Message]", contractTemplateparams.Message);

                    DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
                    dataAccess.setPropertyContractTemplate(xmlParams, embeddedTemplate.TemplateId, embeddedTemplate.EditUrl, embeddedTemplate.ExpiresAt.ToString());

                    resp.entidade = embeddedTemplate.EditUrl;
                }
                else
                {
                    resp.idErro = -1;
                    resp.msgErro = "Impossible to create the template draft.";
                }

                   
            }
            catch (Exception ex)
            {
                resp.idErro = -1;
                resp.msgErro = ex.Message;
            }

            return resp;
        }

        public RespostaContract<string> GetEditTemplateUrl(string templateid)
        {
            RespostaContract<string> resp = new RespostaContract<string>();

            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                EmbeddedTemplate template = client.GetEditUrl(templateid,false,false,true);

                resp.entidade = template.EditUrl;
            }
            catch (Exception ex)
            {
                resp.idErro = -1;
                resp.msgErro = ex.InnerException.Message;
            }

            return resp;
        }


    }
}