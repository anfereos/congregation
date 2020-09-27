using Congregation.Common.Responses;
using Congregation.Common.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace Congregation.Prism.ViewModels
{
    public class MembersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<MemberResponse> _members;

        public MembersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Members";
            LoadMembersAsync();
        }

        public ObservableCollection<MemberResponse> Members
        {
            get => _members;
            set => SetProperty(ref _members, value);
        }

        private async void LoadMembersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<MemberResponse>(url, "/api", "/Members");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            List<MemberResponse> _myMembers = (List<MemberResponse>)response.Result;
            Members = new ObservableCollection<MemberResponse>(_myMembers);
        }
    }
}
