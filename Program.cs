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

        var subFieldOptions = new SubFieldOptions(
            dateFormat: SubFieldOptions.DateFormatEnum.DDMMYYYY
        );

        var metadata = new Dictionary<string, object>()
        {
            ["custom_id"] = 1234,
            ["custom_text"] = "NDA #9"
        };
        //string pdfFilePath = "~/Downloads/example_signature_request.pdf";
        Stream streamEntrada = File.OpenRead(@"example_signature_request_.pdf");
        //byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

        var data = new SignatureRequestSendRequest(
            title: "Flamengo Campeao.",
            subject: "The NDA we talked about",
            message: "Please sign this NDA and then we can discuss more. Let me know if you have any questions.",
            signers: new List<SubSignatureRequestSigner>(){signer1, signer2},
            //ccEmailAddresses: new List<string>(){"douglas.gouveia.medilab@gmail.com"},
           // fileUrl: new List<string>(){"https://app.hellosign.com/docs/example_signature_request.pdf"},
          //  file: new List<Stream>().Add('~/Downloads/example_signature_request.pdf'),
            file: new List<Stream>(){streamEntrada},
            metadata: metadata,
            signingOptions: signingOptions,
            fieldOptions: subFieldOptions,
            testMode: true,
            useTextTags: true
        );

        try
        {
            var result = apiInstance.SignatureRequestSend(data);
            Console.WriteLine(result);
        }
        catch (ApiException e)
        {
            Console.WriteLine("Exception when calling HelloSign API: " + e.Message);
            Console.WriteLine("Status Code: " + e.ErrorCode);
            Console.WriteLine(e.StackTrace);
        }
    }
}
