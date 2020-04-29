using DotPush.Services;
using System.Threading.Tasks;

namespace DotPush
{
    internal interface IPushDotClient
    {
        Task<DotPushHttpResponse> SendiOSMdmPushAsync(MdmPushRequest dotPushHttpRequest);

    }
}
