//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using Realizer.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Realizer.ViewModels
//{
//    [QueryProperty(nameof(Client),  nameof(Client))]
//    public partial class ClientIndivViewModel : ObservableObject
//    {
//        public ClientIndivViewModel() { }

//        [ObservableProperty]
//        private Client _client;

//        [RelayCommand]
//        private async void Back()
//        {
//            await Shell.Current.GoToAsync("//ClientsPage");
//        }
//    }
//}
