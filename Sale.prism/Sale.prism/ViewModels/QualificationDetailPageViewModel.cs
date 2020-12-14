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
    public class QualificationDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationservice;
        private QualificationResponse _qualification;
        public QualificationDetailPageViewModel(INavigationService navigationservice ):base(navigationservice)
        {
          _navigationservice = navigationservice;
            Title = Languages.Qualification;
        }
        public QualificationResponse Qualification
        {
            get => _qualification;
            set => SetProperty(ref _qualification, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters); 
            if(parameters.ContainsKey("qualification"))
            {
                Qualification = parameters.GetValue<QualificationResponse>("qualification");
            }
        }
    }
}
