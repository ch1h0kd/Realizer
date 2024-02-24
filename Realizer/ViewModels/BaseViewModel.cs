using CommunityToolkit.Mvvm.ComponentModel;
using Realizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService NavigationService;
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
