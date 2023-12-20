
namespace CallingAPIAADTest
{
    public interface IDownstreamAPIClient
    {
        Task<CallingAPIResponse> CallDownstreamAPI();
    }
}