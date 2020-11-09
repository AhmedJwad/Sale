using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.Common.Helpers;
using Sale.Common.Models;
using Sale.Common.Responses;
using Sale.prism.Helpers;
using Sale.prism.ItemViewModels;
using Sale.prism.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sale.prism.ViewModels
{
    public class OnSaleMasterDetailPageViewModel :ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;

        public OnSaleMasterDetailPageViewModel(INavigationService navigationService):base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();

        }
     
        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        private void LoadUser()
        {
           if(Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
          {
              new Menu
              {
                  Title=Languages.Products,
                  PageName=$"{nameof(ProductsPage)}",
                  Icon="ic_action_card_giftcard",
              },
              new Menu
              {
                  Title=Languages.ShowShoppingCar,
                  PageName=$"{nameof(ShowCarPage)}",
                  Icon="ic_action_shopping_cart",
              },
              new Menu
              {
                  Title=Languages.ShowPurchaseHistory,
                  PageName=$"{nameof(ShowHistoryPage)}",
                  Icon="ic_action_history_edu",
              },
              new Menu
              {
                  Title=Languages.ModifyUser,
                  PageName=$"{nameof(ModifyUserPage)}",
                  Icon="ic_action_person",
              },
              new Menu
              {
                  Title = Settings.IsLogin ? Languages.Logout : Languages.Login,
                  PageName=$"{nameof(LoginPage)}",
                  Icon="ic_action_exit_to_app",
              },
          };
            Menus = new ObservableCollection<MenuItemViewModel>(menus.Select
                (m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    IsLoginRequired = m.IsLoginRequired,
                    PageName = m.PageName,
                    Title = m.Title,
                }).ToList());
        }
    }
}
