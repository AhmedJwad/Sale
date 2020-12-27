using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Entities;
using Sale.Common.Helpers;
using Sale.Common.Models;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using Sale.prism.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sale.prism.ViewModels
{
    public class AddToCartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _iapiservice;
        private bool _isRunning;
        private bool _isEnabled;
        private ObservableCollection<ProductImage> _images;
        private ProductResponse _product;
        private DelegateCommand _addToCartCommand;      
      
        public AddToCartPageViewModel(INavigationService navigationService, IApiService iapiservice) : base(navigationService)
        {
            _navigationService = navigationService;
           _iapiservice = iapiservice;
            Title = Languages.AddToCart;       
            IsEnabled = true;           
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
     
        public ObservableCollection<ProductImage> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }
        public ProductResponse Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }
        public float Quantity { get; set; }
        public string Remarks { get; set; }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<ProductResponse>("product");
                Images = new ObservableCollection<ProductImage>(Product.ProductImages);
            }
        }
        public DelegateCommand AddToCartCommand => _addToCartCommand ??
            (_addToCartCommand = new DelegateCommand(AddToCartAsync));

        private async void AddToCartAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }        
                       
            List<OrderDetail> orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(Settings.OrderDetails);
            if(orderDetails==null)
            {
                orderDetails = new List<OrderDetail>();
            }
            foreach (var orderDetail in orderDetails)
            {
                if(orderDetail.Product.Id==Product.Id)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ProductExistInOrder, Languages.Accept);
                    await _navigationService.GoBackAsync();
                    return;
                }
            }

            orderDetails.Add(new OrderDetail
            {
                Product = Product,
                Quantity = Quantity,
                Remarks = Remarks,
              
            });
           
            Settings.OrderDetails = JsonConvert.SerializeObject(orderDetails);         

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.AddToCartMessage, Languages.Accept);
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (Quantity == 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.QuantityError, Languages.Accept);
                return false;
            }

            return true;

        }
    }

   
  
    }


