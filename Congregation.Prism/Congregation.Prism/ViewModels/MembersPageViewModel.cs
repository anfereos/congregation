using Congregation.Common.Helpers;
using Congregation.Common.Responses;
using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Congregation.Prism.ViewModels
{
    public class MembersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<UserResponse> _members;
        private bool _isRunning;
        private string _search;
        private List<UserResponse> _myMembers;
        private DelegateCommand _searchCommand;

        public MembersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Members;
            LoadMembersAsync();
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowMembers));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowMembers();
            }
        }

        public ObservableCollection<UserResponse> Members
        {
            get => _members;
            set => SetProperty(ref _members, value);
        }

        private async void LoadMembersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListMembersAsync<UserResponse>(url, "/api", "/Members", token.Token);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message,Languages.Accept);
                return;
            }

            _myMembers = (List<UserResponse>)response.Result;
            ShowMembers();

        }

        private object LogingPage()
        {
            throw new NotImplementedException();
        }

        private void ShowMembers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Members = new ObservableCollection<UserResponse>(_myMembers);
            }
            else
            {
                Members = new ObservableCollection<UserResponse>(_myMembers
                    .Where(m => m.FullName.ToLower().Contains(Search.ToLower())));
            }
        }
    }
}
