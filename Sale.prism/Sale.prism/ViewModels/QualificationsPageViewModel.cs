
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sale.Common.Helpers;
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
    public class QualificationsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isRefreshing;
        private ProductResponse _product;
        private ObservableCollection<QualificationItemViewModel> _qualifications;
        private DelegateCommand _addQualificationCommand;
        public QualificationsPageViewModel(INavigationService navigationService ):base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.Qualification;
        }
        public DelegateCommand AddQualificationCommand => _addQualificationCommand ??
         (_addQualificationCommand = new DelegateCommand(AddQualification));
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public ObservableCollection<QualificationItemViewModel> Qualifications
        {
            get => _qualifications;
            set => SetProperty(ref _qualifications, value);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);   
            if(parameters.ContainsKey("product"))
            {
                LoadProduct(parameters);

            }
        }

        

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("product"))
            {
                LoadProduct(parameters);
            }
        }
        private void LoadProduct(INavigationParameters parameters)
        {
                IsRefreshing = true;
                _product = parameters.GetValue<ProductResponse>("product");
                if (_product.Qualifications != null)
                {
                    Qualifications = new ObservableCollection<QualificationItemViewModel>(_product.Qualifications
                        .Select(q => new QualificationItemViewModel(_navigationService)
                        {
                            Date = q.Date,
                            Id = q.Id,
                            Remarks = q.Remarks,
                            Score = q.Score,
                        }).OrderByDescending(q => q.Date).ToList());
                }
                IsRefreshing = false;
            }
     

        private async void AddQualification()
        {
            if (Settings.IsLogin)
            {
                await _navigationService.NavigateAsync(nameof(AddQualificationPage));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginFirstMessage, Languages.Accept);
                NavigationParameters parameters = new NavigationParameters
                {
                    { "pageReturn", nameof(AddQualificationPage) }
                };

                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            }

        }
    }
    }

