
using DotPush.Services;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DotPush
{
    public class PushDotClient : IPushDotClient
    {
        public async Task<DotPushHttpResponse> SendiOSMdmPushAsync(MdmPushRequest mdmPushRequest)
        {

            MdmPushService mdmPushService = new MdmPushService();
            return await mdmPushService.SendMdmPush(mdmPushRequest);


        }



    }






    public class DotPushHttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Body { get; set; }
        public HttpRequestHeaders RequestHeaders { get; set; }
        public HttpResponseHeaders ResponseHeaders { get; set; }
        public Uri Uri { get; set; }
        public object ResponseData { get; set; }
        public Exception DotPushException { get; set; }
    }
}
