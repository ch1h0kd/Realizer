using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.ViewModels
{
    [QueryProperty(nameof(Product), nameof(Product))]

    public partial class ProductIndivViewModel : ObservableObject
    {
        public  ProductIndivViewModel(){ }

        [ObservableProperty]
        private Product _product;

        [RelayCommand]
        private async void BackProducts()
        {
            await Shell.Current.GoToAsync("//ProductsPage");
        }
    }


}
