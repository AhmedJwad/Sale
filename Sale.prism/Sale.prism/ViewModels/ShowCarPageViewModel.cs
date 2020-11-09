using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.prism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sale.prism.ViewModels
{
    public class ShowCarPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ShowCarPageViewModel(INavigationService navigationService):base(navigationService)
        {
           _navigationService = navigationService;
            Title = Languages.ShowShoppingCar;
        }
    }
}
