using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.prism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sale.prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _nagationservice;

        public ModifyUserPageViewModel(INavigationService nagationservice):base(nagationservice)
        {
            _nagationservice = nagationservice;
            Title = Languages.ModifyUser;
        }
    }
}
