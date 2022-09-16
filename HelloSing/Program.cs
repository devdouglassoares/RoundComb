using System;
using System.Collections.Generic;
using HelloSign.Api;
using HelloSign.Client;
using HelloSign.Model;

public class Example
{
    public static void Main()
    {
        var config = new Configuration();
        // Configure HTTP basic authorization: api_key
        config.Username = "c08b2607e4414a1b6058dcf9ae9260b6634cbc9c790408c26201f18f4e25bdbf";

        // or, configure Bearer (JWT) authorization: oauth2
        // config.AccessToken = "YOUR_BEARER_TOKEN";

        var apiInstance = new SignatureRequestApi(config);
        var _apiInstance = new EmbeddedApi(config);

        var signer1 = new SubSignatureRequestSigner(
            emailAddress: "douglassoaresseven@gmail.com",
            name: "Dougras",
            order: 0
        );

        var signer2 = new SubSignatureRequestSigner(
            emailAddress: "douglassoaresseven@gmail.com",
            name: "Zico",
            order: 1
        );

        var signingOptions = new SubSigningOptions(
            draw: true,
            type: true,
            upload: true,
            phone: true,
            defaultType: SubSigningOptions.DefaultTypeEnum.Draw
        );

        Stream streamEntrada = File.OpenRead(@"example_signature_request_.pdf");

        var data = new SignatureRequestCreateEmbeddedRequest(
            clientId: "5bad73f4b04d61a61b823d07f4a2a0e1",
            title: "Flamengo Campeao.",
            subject: "The NDA we talked about",
            message: "Please sign this NDA and then we can discuss more. Let me know if you have any questions.",
            signers: new List<SubSignatureRequestSigner>(){signer1, signer2},
            ccEmailAddresses: new List<string>(){"douglas.gouveia.medilab@gmail.com"},
            //fileUrl: new List<string>(){"https://app.hellosign.com/docs/example_signature_request.pdf"},
            file: new List<Stream>(){streamEntrada},
            signingOptions: signingOptions,
            testMode: true,
            useTextTags: true
        );

        try
        {
            var result = apiInstance.SignatureRequestCreateEmbedded(data);
            //var result = apiInstance.SignatureRequestSend(data);
            Console.WriteLine(result);
            var signerId1 = result.SignatureRequest.Signatures[0].SignatureId;
            var signerId2 = result.SignatureRequest.Signatures[1].SignatureId;
            Console.WriteLine(signerId1);
            Console.WriteLine(signerId2);
              if(!string.IsNullOrWhiteSpace(signerId1))
            {
            var URL = _apiInstance.EmbeddedSignUrl(signerId1);
            Console.WriteLine(URL);            
            }
           /*  if(!string.IsNullOrWhiteSpace(signerId2))
            {
            var URL2 = _apiInstance.EmbeddedSignUrl(signerId2);
            Console.WriteLine(URL2);
            } */
            /*var _result = apiInstance.SignatureRequestList(accountId);
            Console.WriteLine(_result.SignatureRequestResponse[SignatureRequests]);*/
        }
        catch (ApiException e)
        {
            Console.WriteLine("Exception when calling HelloSign API: " + e.Message);
            Console.WriteLine("Status Code: " + e.ErrorCode);
            Console.WriteLine(e.StackTrace);
        }
    }
}