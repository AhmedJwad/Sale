using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Helpers;
using Sale.Common.Models;
using Sale.Common.Responses;
using Sale.Common.Services;
using Sale.prism.Helpers;
using Sale.prism.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Sale.prism.ViewModels
{
   public  class FinishOrderPageViewModel:ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ICombosHelper _combosHelper;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private decimal _totalValue;
        private int _totalItems;
        private float _totalQuantity;
        private ObservableCollection<Common.Models.PaymentMethod> _paymentMethods;
        private Common.Models.PaymentMethod _paymentMethod;
        private string _deliveryAddress;
        private List<OrderDetailResponse> _orderDetails;
        private TokenResponse _token;
        private DelegateCommand _finishOrderCommand;
        public FinishOrderPageViewModel(INavigationService navigationService,
            ICombosHelper combosHelper, IApiService apiService):base(navigationService)
        {
           _navigationService = navigationService;
           _combosHelper = combosHelper;
            _apiService = apiService;
            Title = Languages.FinishOrder;
            IsEnabled = true;
            PaymentMethods = new ObservableCollection<Common.Models.PaymentMethod>(_combosHelper.GetPaymentMethods());
        }
        public string Remarks { get; set; }
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
        public decimal TotalValue
        {
            get => _totalValue;
            set => SetProperty(ref _totalValue, value);
        }
        public int TotalItems
        {
            get => _totalItems;
            set => SetProperty(ref _totalItems, value);
        }
        public float TotalQuantity
        {
            get => _totalQuantity;
            set => SetProperty(ref _totalQuantity, value);
        }
        public ObservableCollection<Common.Models.PaymentMethod> PaymentMethods
        {
            get => _paymentMethods;
            set => SetProperty(ref _paymentMethods, value);
        }
        public Common.Models.PaymentMethod PaymentMethod
        {
            get => _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }
        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set => SetProperty(ref _deliveryAddress, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadOrderTotals();            
        }

        private  void LoadOrderTotals()
        {
            _token= JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
           _orderDetails = JsonConvert.DeserializeObject<List<OrderDetailResponse>>(Settings.OrderDetails);
            if(_orderDetails == null)
            {
                _orderDetails = new List<OrderDetailResponse>();
            }
            TotalItems = _orderDetails.Count;
            TotalValue = _orderDetails.Sum(od => od.Value).Value;
            TotalQuantity = _orderDetails.Sum(od => od.Quantity);
            DeliveryAddress = $"{_token.User.Address},{_token.User.City.Name}";
        }
        public DelegateCommand FinishOrderCommand => _finishOrderCommand ??
            (_finishOrderCommand = new DelegateCommand(FinishOrderAsync));

        private async void FinishOrderAsync()
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

            OrderResponse request = new OrderResponse
            {
                OrderDetails=_orderDetails,
                PaymentMethod = ToPaymentMethod(PaymentMethod),
                Remarks = Remarks
            };
            Response response = await _apiService.PostAsync(url, "api", "/Orders", request, _token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _orderDetails.Clear();
            Settings.OrderDetails = JsonConvert.SerializeObject(_orderDetails);
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.FinishOrderMessage, Languages.Accept);
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
        }

        private Common.Enums.PaymentMethod ToPaymentMethod(Common.Models.PaymentMethod paymentMethod)
        {
            switch (paymentMethod.Id)
            {
                case 1: return Common.Enums.PaymentMethod.Cash;
                default: return Common.Enums.PaymentMethod.CreditCard;               
            }

        }

        private async Task<bool> ValidateDataAsync()
        {
            if (PaymentMethod == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PaymentMethodError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(DeliveryAddress))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DeliveryAddressError, Languages.Accept);
                return false;
            }

            return true;
        }
    }
}
