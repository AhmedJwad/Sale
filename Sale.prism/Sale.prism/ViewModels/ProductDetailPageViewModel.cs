﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.Common.Entities;
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
    public class ProductDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ProductResponse _product;
        private ObservableCollection<ProductImage> _images;
        private DelegateCommand _addToCartBinding;

        public ProductDetailPageViewModel(INavigationService navigationService):base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.Details;
        }

        public ObservableCollection<ProductImage> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }
        public ProductResponse product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters); 
            if(parameters.ContainsKey("product"))
            {
                product = parameters.GetValue<ProductResponse>("product");
                Title = product.Name;
                Images = new ObservableCollection<ProductImage>(product.ProductImages);
            }
        }
        public DelegateCommand AddToCartBinding => _addToCartBinding ??
            (_addToCartBinding = new DelegateCommand(AddToCartasync));

        private async void AddToCartasync()
        {
           NavigationParameters parameters = new NavigationParameters
            {
                { "product", product }
            };
            await _navigationService.NavigateAsync(nameof(AddToCartPage), parameters);
        }
    }
}
