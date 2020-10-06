using Congregation.Common.Request;
using Congregation.Common.Responses;
using System.Threading.Tasks;

namespace Congregation.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
    }

}
