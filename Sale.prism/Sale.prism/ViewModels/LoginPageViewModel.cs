using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.Common.Helpers;
using Sale.Common.Request;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using Sale.prism.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace Sale.prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private string _pageReturn;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        public LoginPageViewModel(INavigationService navigationService, IApiService apiService):base(navigationService)
        {
            _navigationService = navigationService;
           _apiService = apiService;
            Title = Languages.Login;
            IsEnabled = true;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("pageReturn"))
            {
                _pageReturn = parameters.GetValue<string>("pageReturn");
            }
        }
        public string Email
        {
            get; set;
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public DelegateCommand LoginCommand => _loginCommand ??
            (_loginCommand = new DelegateCommand(LoginAsync));       
    
        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Accept);
                return;
            }
            IsRunning = true;
            IsEnabled = false;
            if(Connectivity.NetworkAccess!=NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.ConnectionError, Languages.Accept);
                return;
            }
            string Url = App.Current.Resources["UrlAPI"].ToString();
            TokenRequest request = new TokenRequest
            {
                Username = Email,
                Password = Password,
            };
            Response response = await _apiService.GetTokenAsync(Url, "/api", "/Account/CreateToken"
                , request);
            if(!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;               
                await App.Current.MainPage.DisplayAlert(Languages.Error,
                   Languages.LoginError, Languages.Accept);
                Password = string.Empty;
                return;
            }
            TokenResponse token = (TokenResponse)response.Result;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;
            Password = string.Empty;
            //await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
            if (string.IsNullOrEmpty(_pageReturn))
            {
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{_pageReturn}");
            }           
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand =
            new DelegateCommand(RegisterAsync));
        private async void RegisterAsync()
        {          

        }

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ??
            (_forgotPasswordCommand = new DelegateCommand(ForgotPasswordAsync));

        private void ForgotPasswordAsync()
        {
            throw new NotImplementedException();
        }

       
     
    }
}
