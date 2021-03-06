﻿using Congregation.Common.Helpers;
using Congregation.Common.Models;
using Congregation.Common.Responses;
using Congregation.Prism.Helpers;
using Congregation.Prism.ItemViewModels;
using Congregation.Prism.Views;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Congregation.Prism.ViewModels
{
    public class CongregationMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private static CongregationMasterDetailPageViewModel _instance;

        public CongregationMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();
        }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public static CongregationMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
        {
            new Menu
            {
                Icon = "ic_action_supervised_user_circle",
                PageName = $"{nameof(MembersPage)}",
                Title = Languages.Members
            },
            new Menu
            {
                Icon = "ic_action_check_circle",
                PageName = $"{nameof(MeetingPage)}",
                Title = Languages.Meetings,
                IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_action_edit",
                PageName = $"{nameof(ModifyUserPage)}",
                Title = Languages.ModifyUser,
                IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = $"{nameof(LoginPage)}",
                Title = Settings.IsLogin ? Languages.Logout : Languages.Login,
                IsLoginRequired = false
            },
            new Menu
            {
                Icon = "ic_action_person_add",
                PageName = $"{nameof(RegisterPage)}",
                Title = Languages.Register,
                IsLoginRequired = false
            }
        };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
        }
    }

}
