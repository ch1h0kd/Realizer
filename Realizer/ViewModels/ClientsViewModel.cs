using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Data;
using Realizer.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Realizer.ViewModels
{
    [QueryProperty(nameof(OperatingClient), nameof(Client))] // receiving a client when moving to Indiv and Edit page
    [QueryProperty(nameof(Clients), nameof(Clients))] // receiving a collection of clients when moving to Indiv and Edit page
    public partial class ClientsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;//instance from DatabaseContext
        private PhoneNumViewModel phoneNumViewModel;

        public ICommand Back_Command { get; set; }
        public ClientsViewModel(DatabaseContext context, PhoneNumViewModel phoneNumviewmodel)
        {
            _context = context;
            Back_Command = new Command(BackToClients);
            phoneNumViewModel = phoneNumviewmodel;
        }

        [ObservableProperty]
        private ObservableCollection<Client> _clients = new();

        [ObservableProperty]
        private Client _operatingClient = new();//client that is currently eddited

        [ObservableProperty]
        private PhoneNumber _operatingNum = new();//number that is currently eddited

        [ObservableProperty]
        private ObservableCollection<PhoneNumber> _operatingNums = new();//numbers that is currently eddited

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        [ObservableProperty]
        private bool _error = false;

        [ObservableProperty]
        private bool _searching;

        [ObservableProperty]
        private DateTime _maxDate = DateTime.Today;

        public async Task LoadClientsAsync()
        {
            await ExecuteAsync(async () =>
            {
                var clients = await _context.GetAllAsync<Client>();

                if (clients is not null && clients.Any())//if we have at least one client
                {
                    Clients ??= new ObservableCollection<Client>(); //if null, initialize

                    foreach (var client in clients)//insert each client into a obervable collection Clients 
                    {

                        Clients.Add(client);
                    }
                }
            }, "Fetching clients...");
        }

        public async Task ValidateClientAsync()
        {
            if (OperatingClient is null)
            {//do nothing
                Error = true;
                return;
            }
            var (isValid, errorMessage) = OperatingClient.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Alert", errorMessage, "Ok");
                Error = true;
                return;
            }
            //var busyText = await _context.GetFilteredAsync<Client>(x => x.Id == OperatingClient.Id) is null ? "New ID" : "ID exists";
            var filtered = await _context.GetFilteredAsync<Client>(x => x.client_id == OperatingClient.client_id);
            if (filtered is not null && filtered.Any())//if we have at least one client in the result
            {
                if ((OperatingClient is null) || (filtered.First().client_key != OperatingClient.client_key))
                {
                    await Shell.Current.DisplayAlert("Alert", "ID already used", "Ok");
                    Error = true;
                    return;
                }
            }
        }

        [RelayCommand]
        private void SetOperatingClient(Client? client) => OperatingClient = client ?? new();

        [RelayCommand]
        private async Task SaveClientAsync()//loading
        {
            //validate and add client
            await ValidateClientAsync();
            if (Error == true)
            {
                Error = false;
                return;
            }
            await _context.AddItemAsync<Client>(OperatingClient);

            //validate and add phone number
            OperatingNums.Add(OperatingNum);
            foreach (var each in OperatingNums)
            {
                if (each.number != null)
                {
                    var (isValid, errorMessage) = each.Validate();
                    if (!isValid)
                    {
                        await Shell.Current.DisplayAlert("Alert", errorMessage, "Ok");
                        Error = true;
                        return;
                    }
                    else if(errorMessage != "empty") {
                        each.client_key = OperatingClient.client_key;
                        await phoneNumViewModel.SavePhoneNumAsync(each);
                    }
                    //when empty, do nothing
                    
                }

            }
            Clients.Add(OperatingClient);//add this client to the collection
            await Shell.Current.GoToAsync("//ClientsPage");

            //SetOperatingClientCommand.Execute(new());//reset the value
        }

        [RelayCommand]
        private async Task UpdateClientAsync()
        {
            //validate and update client
            await ValidateClientAsync();
            if (Error == true)
            {
                Error = false;
                return;
            }
            await _context.UpdateItemAsync(OperatingClient);

            //update phone number
            if (OperatingNums != null && OperatingNums.Any())
            {
                foreach (var each in OperatingNums)
                {
                    if (each.number != null)
                    {
                        var (isValid, errorMessage) = each.Validate();
                        if (!isValid)
                        {
                            await Shell.Current.DisplayAlert("Alert", errorMessage, "Ok");
                            Error = true;
                            return;
                        }
                        else if(errorMessage == "empty")
                        {
                            await phoneNumViewModel.DeletePhoneNumAsync(each.phoneNum_id);
                        }
                        else if(each.client_key == 0)
                        {
                            each.client_key = OperatingClient.client_key;
                            await phoneNumViewModel.SavePhoneNumAsync(each);
                        }
                        else await phoneNumViewModel.UpdatePhoneNumAsync(each);
                    }
                }
            }

            GoToClientIndiv(OperatingClient);
        }

        [RelayCommand]
        private async Task DeleteClientAsync(int key)
        {
            bool answer = await Shell.Current.DisplayAlert("Are you sure?", "Other information involving this client will not be deleted", "Delete", "Cancel");
            if (answer == true)
            {
                await ExecuteAsync(async () =>
                {
                    if (await _context.DeteleItemByKeyAsync<Client>(key))
                    {
                        var client = Clients.FirstOrDefault(p => p.client_key == key);//get the item
                        Clients.Remove(client);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Delete Error", "Client was not deleted", "Ok");
                    }
                }, "Deleting client...");
                await Shell.Current.GoToAsync("//ClientsPage");
            }
            else return;
        }

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

        [RelayCommand]
        private async Task SearchClients(string searchTerm)

        {
            Clients.Clear();
            Searching = true;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                await LoadClientsAsync();
                return;
            }
            //var client = _context.GetFilteredAsync<Client>(x => x.Name == searchTerm);
            foreach (var client in await _context.GetFilteredAsync<Client>(x => x.client_name.Contains(searchTerm)))
            {
                Clients.Add(client);
            }
            Searching = false;
        }

        [RelayCommand]
        private async void GoToClientIndiv(Client client) //passing a client and collection of clients
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(Client)] = client, //passing a client
                [nameof(Clients)] = Clients //passing a collection of clients
            };
            await Shell.Current.GoToAsync("//ClientIndivPage", animate: true, parameter);
        }

        [RelayCommand]
        private async void GoToClientEdit(Client client) //passing a client and collection of clients
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(Client)] = client, //passing a client
                [nameof(Clients)] = Clients //passing a collectio of clients
            };
            await Shell.Current.GoToAsync("//ClientEditPage", animate: true, parameter);
        }

        [RelayCommand]
        private async void BackToClients()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async void Back()
        {
            await Shell.Current.GoToAsync("//ClientsPage");
        }

    }


}