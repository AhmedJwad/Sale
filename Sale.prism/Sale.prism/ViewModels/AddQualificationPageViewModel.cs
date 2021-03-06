﻿using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.Common.Helpers;
using Sale.Common.Request;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace Sale.prism.ViewModels
{
    public class AddQualificationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isEnabled;
        private bool _isRunning;
        private DelegateCommand _saveCommand;
        public AddQualificationPageViewModel(INavigationService navigationService, IApiService apiService ):base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.NewQualification;
            IsEnabled = true;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public float Qualification { get; set; }

        public string Remarks { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand(Save));

        private async void Save()
        {
           if(Qualification==0)
            {
                await App.Current.MainPage.DisplayAlert
                    (Languages.Error, Languages.QualificationError, Languages.Accept);
                return;
            }
            IsRunning = true;
            IsEnabled = false;
            if(Connectivity.NetworkAccess!=NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.ConnectionError, Languages.Accept);
                return;
            }
            ProductResponse myproduct = JsonConvert.DeserializeObject<ProductResponse>(Settings.Product);
            string url = App.Current.Resources["UrlAPI"].ToString();
            QualificationRequest request = new QualificationRequest
            {
                ProductId = myproduct.Id,
                Remarks = Remarks,
                Score = Qualification,
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.PostQualificationAsync(url, "api", "/Qualifications"
                , request, token.Token);
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            ProductResponse product = (ProductResponse)response.Result;
            NavigationParameters parameters = new NavigationParameters
            {
                  { "product", product }
            };
            await _navigationService.GoBackAsync(parameters);
        }
    }
}
