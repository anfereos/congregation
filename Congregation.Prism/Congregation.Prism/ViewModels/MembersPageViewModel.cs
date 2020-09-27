using Congregation.Common.Responses;
using Congregation.Common.Services;
using Congregation.Prism.Helpers;
using Prism.Commands;
using Prism.Navigation;
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
        private ObservableCollection<MemberResponse> _members;
        private bool _isRunning;

        private string _search;
        private List<MemberResponse> _myMembers;
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

            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<MemberResponse>(url, "/api", "/Members");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            _myMembers = (List<MemberResponse>)response.Result;
            ShowMembers();

        }

        private void ShowMembers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Members = new ObservableCollection<MemberResponse>(_myMembers);
            }
            else
            {
                Members = new ObservableCollection<MemberResponse>(_myMembers
                    .Where(m => m.FullName.ToLower().Contains(Search.ToLower())));
            }
        }


    }
}
