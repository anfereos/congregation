using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Prism.Navigation;

namespace Congregation.Prism.ViewModels
{
    public class AssistancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        public AssistancePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Assistance;
        }
    }
}
