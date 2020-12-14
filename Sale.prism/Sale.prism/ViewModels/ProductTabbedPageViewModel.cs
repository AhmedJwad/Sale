using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.Common.Responses;
using Sale.prism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sale.prism.ViewModels
{
    public class ProductTabbedPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ProductTabbedPageViewModel(INavigationService navigationService ):base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.Product;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters); 
            if(parameters.ContainsKey("product"))
            {
                ProductResponse product = parameters.GetValue<ProductResponse>("product");
                Title = product.Name;

            }
        }
    }
}
