using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Data;
using Realizer.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Realizer.ViewModels
{
    [QueryProperty(nameof(OperatingClientId), nameof(Client.client_id))] // receiving a client
    public partial class PhoneNumViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;//instance from DatabaseContext
        //public ICommand Back_Command { get; set; }
        public PhoneNumViewModel(DatabaseContext context)
        {
            _context = context;
            //Back_Command = new Command(BackToClients);
        }

        [ObservableProperty]
        private ObservableCollection<PhoneNumber> _operatingNum = new();//no need?

        [ObservableProperty]
        private ObservableCollection<PhoneNumber> _operatingNums = new();//client that is currently eddited

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        [ObservableProperty]
        private int _operatingClientId;

        //[ObservableProperty]
        //private bool _searching;

        //[ObservableProperty]
        //private DateTime _maxDate = DateTime.Today;

        //loadPhone needs id, load client is not called
        public async Task LoadPhoneNumByIdAsync(int client_key)
        {
            var filtered = await _context.GetFilteredAsync<PhoneNumber>(x => x.client_key == client_key);
            if (filtered is not null && filtered.Any())
            {
                //_operatingNums ??= new ObservableCollection<PhoneNumber>(); //if null, initialize

                foreach (var num in filtered)
                {
                    OperatingNums.Add(num);
                }
            }
        }

        //[RelayCommand]
        //private void SetOperatingPhoneNum(ObservableCollection<PhoneNumber>? phoneNum) => OperatingNums = phoneNum ?? new();

        //save a PhoneNubmer object with given client_id and phone number
        //is called after validation, number.number is not null
        public async Task SavePhoneNumAsync(PhoneNumber number)
        {
            var filtered = await _context.GetFilteredAsync<PhoneNumber>(x => x.client_key == number.client_key && x.number == number.number);
            if (filtered == null)
            {
                Console.WriteLine("GetFilteredAsync returned null");
                return;
            }
            if (filtered != null && filtered.Any())
            {
                return;
            }
            await _context.AddItemAsync(number);//create
        }

        //validate and update
        public async Task UpdatePhoneNumAsync(PhoneNumber num)
        {
            await _context.UpdateItemAsync(num);
            //foreach (var each in OperatingNums)
            //{
            //    var (isValid, errorMessage) = each.Validate();
            //    if (!isValid)
            //    {
            //        await Shell.Current.DisplayAlert("Alert", errorMessage, "Ok");
            //        //Error = true;
            //        return;
            //    }
            //    await _context.UpdateItemAsync(each);
            //}

        }

        //[RelayCommand]
        //private async Task DeletePhoneNumAsync(int key)
        //{
        //    bool answer = await Shell.Current.DisplayAlert("Are you sure?", "Other information involving this client will not be deleted", "Delete", "Cancel");
        //    if (answer == true)
        //    {
        //        await ExecuteAsync(async () =>
        //        {
        //            if (await _context.DeteleItemByKeyAsync<Client>(key))
        //            {
        //                var client = Clients.FirstOrDefault(p => p.client_key == key);//get the item
        //                Clients.Remove(client);
        //            }
        //            else
        //            {
        //                await Shell.Current.DisplayAlert("Delete Error", "Client was not deleted", "Ok");
        //            }
        //        }, "Deleting client...");
        //        await Shell.Current.GoToAsync("//ClientsPage");
        //    }
        //    else return;
        //}


        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();
            }
            finally
            {
                IsBusy = false;//after the operations, not busy anymore
                BusyText = "Processing...";
            }
        }

        //[RelayCommand]
        //private async Task SearchClients(string searchTerm)

        //{
        //    Clients.Clear();
        //    Searching = true;
        //    if (string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        await LoadClientsAsync();
        //        return;
        //    }
        //    //var client = _context.GetFilteredAsync<Client>(x => x.Name == searchTerm);
        //    foreach (var client in await _context.GetFilteredAsync<Client>(x => x.client_name.Contains(searchTerm)))
        //    {
        //        Clients.Add(client);
        //    }
        //    Searching = false;
        //}

        //[RelayCommand]
        //private async void GoToClientIndiv(Client client)
        //{
        //    var parameter = new Dictionary<string, object>
        //    {
        //        [nameof(Client)] = client,
        //        [nameof(Clients)] = Clients
        //    };
        //    await Shell.Current.GoToAsync("//ClientIndivPage", animate: true, parameter);
        //}

        //[RelayCommand]
        //private async void GoToClientEdit(Client client)
        //{
        //    var parameter = new Dictionary<string, object>
        //    {
        //        [nameof(Client)] = client,
        //        [nameof(Clients)] = Clients
        //    };
        //    await Shell.Current.GoToAsync("//ClientEditPage", animate: true, parameter);
        //}

        //[RelayCommand]
        //private async void BackToClients()
        //{
        //    await Shell.Current.GoToAsync("..");
        //}

        //[RelayCommand]
        //private async void Back()
        //{
        //    await Shell.Current.GoToAsync("//ClientsPage");
        //}

    }
}