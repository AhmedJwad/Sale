using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Request;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Sale.prism.ViewModels
{
   public class RecoverPasswordPageViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private string _email;
        private DelegateCommand _recoverCommand;
        public RecoverPasswordPageViewModel(INavigationService navigationService,
            IRegexHelper regexHelper,
            IApiService apiService):base(navigationService)
        {
            _navigationService = navigationService;
           _regexHelper = regexHelper;
            _apiService = apiService;
            Title = Languages.RecoverPassword;
            IsEnabled = true;
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
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
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters); 
            if(parameters.ContainsKey("emial"))
            {
                Email = parameters.GetValue<string>("emial");
            }
        }
        public DelegateCommand RecoverCommand => _recoverCommand ?? (_recoverCommand
            = new DelegateCommand(Recoverpasswordasync));

        private async void Recoverpasswordasync()
        {
            bool isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            emailrequest emailrequest = new emailrequest
            {
                Email = Email,
            };
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.RecoverPasswordAsync(url, "/api", "/Account/RecoverPasswordApp", emailrequest);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error001")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error001, Languages.Accept);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.RecoverPasswordMessage, Languages.Accept);
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Email) || !_regexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return false;
            }

            return true;
        }

    }
}
