using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Sale.Common.Helpers;
using Sale.Common.Responses;
using Sale.prism.Views;

namespace Sale.prism.ItemViewModels
{
    public class ProductItemViewModel:ProductResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectProductCommand;
        private DelegateCommand _selectProduct2Command;
        public ProductItemViewModel(INavigationService navigationService)
        {
          _navigationService = navigationService;
        }
        public float Quantity { get; set; }

        public string Remarks { get; set; }

        public decimal Value => (decimal)Quantity * Price;


        public DelegateCommand SelectProductCommand => _selectProductCommand ??
            (_selectProductCommand = new DelegateCommand(SelectProductAsync));

        private async void SelectProductAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "product" , this}
            };
            Settings.Product = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync(nameof(ProductTabbedPage), parameters);
        }
        public DelegateCommand SelectProduct2Command => _selectProduct2Command ??
          (_selectProduct2Command = new DelegateCommand(SelectProduct2async));

        private void SelectProduct2async()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "product" , this}
            };
            Settings.Product = JsonConvert.SerializeObject(this);
            _navigationService.NavigateAsync(nameof(ModifiyOrderPage),parameters);
        }
    }
}
