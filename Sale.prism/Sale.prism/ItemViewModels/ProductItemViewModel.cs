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

        public ProductItemViewModel(INavigationService navigationService)
        {
          _navigationService = navigationService;
        }

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
    }
}
