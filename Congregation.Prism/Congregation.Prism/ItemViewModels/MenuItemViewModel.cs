using Congregation.Common.Helpers;
using Congregation.Common.Models;
using Congregation.Prism.Helpers;
using Congregation.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace Congregation.Prism.ItemViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (PageName == nameof(LoginPage) && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.Token = null;
            }

            await _navigationService.NavigateAsync($"/{nameof(CongregationMasterDetailPage)}/NavigationPage/{PageName}");

            if (IsLoginRequired && !Settings.IsLogin)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginFirstMessage, Languages.Accept);
                NavigationParameters parameters = new NavigationParameters
                {
                    { "pageReturn", PageName }
                };

                await _navigationService.NavigateAsync($"/{nameof(CongregationMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(CongregationMasterDetailPage)}/NavigationPage/{PageName}");
            }

        }
    }
}