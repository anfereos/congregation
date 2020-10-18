using Prism;
using Prism.Ioc;
using Congregation.Prism.ViewModels;
using Congregation.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Congregation.Common.Services;
using Syncfusion.Licensing;
using Congregation.Prism.Helpers;
using Congregation.Common.Helpers;
using Congregation.Common.Responses;
using Newtonsoft.Json;

namespace Congregation.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzI4NzMyQDMxMzgyZTMzMmUzMEwwTEN2cTkvTndzb3lxNjZ0SmFxbGJkTTAyK3lWaWoyYUNJQzlzdC94eU09");

            InitializeComponent();

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            if (token == null)
            {
                await NavigationService.NavigateAsync($"{nameof(CongregationMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(CongregationMasterDetailPage)}/NavigationPage/{nameof(MembersPage)}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CongregationMasterDetailPage, CongregationMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MembersPage, MembersPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingPage, MeetingPageViewModel>();

            containerRegistry.RegisterForNavigation<AddMeetingPage, AddMeetingPageViewModel>();
            containerRegistry.RegisterForNavigation<UpdateMeetingPage, UpdateMeetingPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
        }
    }
}
