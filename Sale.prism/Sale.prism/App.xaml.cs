using Prism;
using Prism.Ioc;
using Sale.prism.ViewModels;
using Sale.prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Sale.Common.Services;
using Syncfusion.Licensing;

namespace Sale.prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjExMTY0QDMxMzcyZTM0MmUzMEc1K2xKbkhrV2RmMHByRXF6YUJDQlQ3RkxLZ3hxOVlyMHY0T1RiSUFEZUk9");
            InitializeComponent();
            await NavigationService.NavigateAsync($"NavigationPage/{nameof(ProductsPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
        }
    }
}
