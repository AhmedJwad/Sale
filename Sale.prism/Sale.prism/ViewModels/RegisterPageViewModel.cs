﻿using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Entities;
using Sale.Common.Request;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sale.prism.ViewModels
{
   public class RegisterPageViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private UserRequest _user;
        private ImageSource _image;
        private City _city;
        private ObservableCollection<City> _cities;
        private Department _department;
        private ObservableCollection<Department> _departments;
        private Country _country;
        public ObservableCollection<Country> _countries;
        private bool _isEnabled;
        private bool _isRunning;
        private DelegateCommand _registerCommand;
        public RegisterPageViewModel(INavigationService navigationService, IRegexHelper regexHelper
            , IApiService apiService):base(navigationService)
        {
           _navigationService = navigationService;
          _regexHelper = regexHelper;
           _apiService = apiService;
            Title = Languages.Register;
            IsEnabled = true;
            Image = App.Current.Resources["UrlNoImage"].ToString();
            User = new UserRequest();
            LoadCountriesAsync();

        }

      

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public City City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }
        public ObservableCollection<City> Cities
        {
            get => _cities;
            set => SetProperty(ref _cities, value);
        }
        public Department Department
        {
            get => _department;
            set
            {
                Cities = value != null ? new ObservableCollection<City>(value.Cities) : null;
                City = null;
                SetProperty(ref _department, value);
            }
        }
        public ObservableCollection<Department> Departments
        {
            get => _departments;
            set => SetProperty(ref _departments, value);
        }

        public Country Country
        {
            get => _country;
            set
            {
                Departments = value != null ? new ObservableCollection<Department>(value.Departments) : null;
                Cities = new ObservableCollection<City>();
                Department = null;
                City = null;
                SetProperty(ref _country, value);
            }
        }
        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }
        private async void LoadCountriesAsync()
        {
            IsRunning = true;
            IsEnabled = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Country>(url, "/api", "/Countries");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            List<Country> list = (List<Country>)response.Result;
            Countries = new ObservableCollection<Country>(list.OrderBy(c => c.Name));

        }
        public DelegateCommand RegisterCommand => _registerCommand ??
          (  _registerCommand = new DelegateCommand(Registerasync));

        private async void Registerasync()
        {
            bool isValid = await ValidateDataAsync();
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
            string url = App.Current.Resources["UrlAPI"].ToString();
            User.CityId = City.Id;
            Response response = await _apiService.RegisterUserAsync(url, "/api", "/Account/Register",
                User);
            IsRunning = false;
            IsEnabled = true;
            if (!response.IsSuccess)
            {
                if (response.Message == "Error003")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error003, Languages.Accept);
                }

                else if (response.Message == "Error004")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error004,
                      Languages.Accept);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                }
                return;
            }
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.RegisterMessge,
                Languages.Accept);
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Phone))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Accept);
                return false;
            }

            if (Country == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.CountryError, Languages.Accept);
                return false;
            }

            if (Department == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DepartmentError, Languages.Accept);
                return false;
            }

            if (City == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.CityError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Password) || User.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError1, Languages.Accept);
                return false;
            }

            if (User.Password != User.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError2, Languages.Accept);
                return false;
            }

            return true;
        }
    }
}
