﻿using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Helpers;
using Sale.Common.Models;
using Sale.prism.Helpers;
using Sale.prism.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sale.prism.ItemViewModels
{
   public class MenuItemViewModel:Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
           _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ??
            (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if(PageName==nameof(LoginPage) && (Settings.IsLogin))
            {
                Settings.IsLogin = false;
                Settings.Token = null;
            }
            if(!IsLoginRequired && !Settings.IsLogin)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.LoginFirstMessage, Languages.Accept);
                NavigationParameters parameters = new NavigationParameters
                {
                    {"pageReturn", PageName }
                };
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}" +
              $"/NavigationPage/{PageName}");
            }          
        }
    }
}
