using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotPush.Services
{
    internal class MdmPushService
    {

        internal async Task<DotPushHttpResponse> SendMdmPush(MdmPushRequest mdmPushRequest)
        {
            DotPushHttpResponse dotPushHttpResponse = new DotPushHttpResponse();

            try
            {
            
                var cert = new X509Certificate2(mdmPushRequest.CertificatePath, mdmPushRequest.CertificatePassword);

                var handler = new HttpClientHandler();

                handler.ClientCertificateOptions = ClientCertificateOption.Manual;

                handler.ClientCertificates.Add(cert);

                string requestUrl = $"https://sandbox.push.apple.com/3/device/{mdmPushRequest.Hextoken}";

                if (mdmPushRequest.IsProduction)
                {
                    requestUrl = $"https://api.push.apple.com/3/device/{mdmPushRequest.Hextoken}";
                }

                var client = new HttpClient(handler);

                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

                request.Version = new Version(2, 0);

                request.Headers.Add("apns-topic", mdmPushRequest.ApnsTopic);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                var json = JsonSerializer.Serialize(mdmPushRequest.ApnsPayload, options);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                request.Content = stringContent;

                HttpResponseMessage response = await client.SendAsync(request);


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var apnsId = response.Headers.TryGetValues("apns-id", out var values) ? values.FirstOrDefault() : null;
                    dotPushHttpResponse.ResponseData = new {Success = true, ApnsId  = apnsId};

                }


                dotPushHttpResponse.Uri = new Uri(requestUrl);
                dotPushHttpResponse.Body = stringContent.ToString();
                dotPushHttpResponse.RequestHeaders = request.Headers;
                dotPushHttpResponse.ResponseHeaders = response.Headers;
                dotPushHttpResponse.StatusCode = response.StatusCode;

               
            }

            catch (Exception exc)
            {

                dotPushHttpResponse.DotPushException = exc;
            }


            return dotPushHttpResponse;



        }
    }


    public class MdmPushRequest
    {
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }
        public string ApnsTopic { get; set; }
        public string Hextoken { get; set; }

        public bool IsProduction { get; set; }

        public object ApnsPayload { get; set; }

    }
}
