using Congregation.Common.Models;
using Congregation.Common.Request;
using Congregation.Common.Responses;
using System.IO;
using System.Threading.Tasks;

namespace Congregation.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<RandomUsers> GetRandomUser(string urlBase, string servicePrefix);

        Task<Stream> GetPictureAsync(string urlBase, string servicePrefix);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> ModifyUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);


    }

}
