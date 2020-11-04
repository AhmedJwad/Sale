using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Entities;
using Sale.Common.Responses;
using Sale.Common.Services;
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
        private List<Product> _myproducts;
        private ObservableCollection<Product> _products;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _searchCommand;

        public ProductsPageViewModel(INavigationService navigationservice ,IApiService apiService):base(navigationservice)
        {
            _navigationservice = navigationservice;
            _apiService = apiService;
            Title = "Products";
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
           if(string.IsNullOrEmpty(Search))
            {
                Products = new ObservableCollection<Product>(_myproducts);
            }
           else
            {
                Products = new ObservableCollection<Product>(_myproducts.
                    Where(p=>p.Name.ToLower().Contains(Search.ToLower())));
            }
        }

        public ObservableCollection<Product>Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        private async void LoadProductsAsync()
        {
            if(Connectivity.NetworkAccess!=NetworkAccess.Internet)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", "there is no internet connection", "Accept");
                return;
            }
            IsRefreshing = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response respons = await _apiService.GetListAsync<Product>(url, "/api", "/Products");
            IsRefreshing = false;
            if(!respons.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("error", respons.Message, "Accept");
                return;
            }
           _myproducts = (List<Product>)respons.Result;
            ShowProducts();         
        }
        public DelegateCommand RefreshCommand => _refreshCommand ??
           (_refreshCommand = new DelegateCommand(LoadProductsAsync));

        public DelegateCommand SearchCommand => _searchCommand ??
            (_searchCommand = new DelegateCommand(ShowProducts));
    }
}
