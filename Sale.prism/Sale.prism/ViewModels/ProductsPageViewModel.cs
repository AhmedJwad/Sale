using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Entities;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using Sale.prism.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Sale.prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationservice;
        private readonly IApiService _apiService;
        private bool _isRefreshing;
        private string _search;
        private List<ProductResponse> _myproducts;
        private ObservableCollection<ProductItemViewModel> _products;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _searchCommand;

        public ProductsPageViewModel(INavigationService navigationservice ,IApiService apiService):base(navigationservice)
        {
            _navigationservice = navigationservice;
            _apiService = apiService;
            Title = Languages.Products;
            LoadProductsAsync();
        }   

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set=> SetProperty(ref _isRefreshing, value);
        }
        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowProducts();
            }
        }

        private void ShowProducts()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Products = new ObservableCollection<ProductItemViewModel>(_myproducts
                    .Select(p => new ProductItemViewModel(_navigationservice)
                    {
                        Category=p.Category,
                        Price=p.Price,
                        Description=p.Description,
                        IsActive=p.IsActive,
                        IsStarred=p.IsStarred,                      
                        Name=p.Name,
                        ProductImages=p.ProductImages,
                        Id=p.Id,
                        Qualifications=p.Qualifications,
                    }).ToList());              
            }
           else
            {
                Products = new ObservableCollection<ProductItemViewModel>(_myproducts
                    .Select(p=> new ProductItemViewModel(_navigationservice) 
                    {
                        Category = p.Category,
                        Price = p.Price,
                        Description = p.Description,
                        IsActive = p.IsActive,
                        IsStarred = p.IsStarred,
                        Name = p.Name,
                        ProductImages = p.ProductImages,
                        Id = p.Id,
                        Qualifications=p.Qualifications,
                    }).Where(p=>p.Name.ToLower().Contains(Search.ToLower())).ToList());
            }
        }

        public ObservableCollection<ProductItemViewModel>Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        private async void LoadProductsAsync()
        {
            if(Connectivity.NetworkAccess!=NetworkAccess.Internet)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError,Languages.Accept);
                return;
            }
            IsRefreshing = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response respons = await _apiService.GetListAsync<ProductResponse>(url, "/api", "/Products");
            IsRefreshing = false;
            if(!respons.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, respons.Message, Languages.Accept);
                return;
            }
           _myproducts = (List<ProductResponse>)respons.Result;
            ShowProducts();         
        }
        public DelegateCommand RefreshCommand => _refreshCommand ??
           (_refreshCommand = new DelegateCommand(LoadProductsAsync));

        public DelegateCommand SearchCommand => _searchCommand ??
            (_searchCommand = new DelegateCommand(ShowProducts));
    }
}
