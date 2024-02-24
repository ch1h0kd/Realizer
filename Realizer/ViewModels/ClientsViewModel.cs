using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Data;
using Realizer.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Realizer.ViewModels
{
    [QueryProperty(nameof(OperatingClient), nameof(Client))]
    [QueryProperty(nameof(Clients), nameof(Clients))]
    public partial class ClientsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;//instance from DatabaseContext
        public ICommand Back_Command { get; set; }
        public ClientsViewModel(DatabaseContext context)
        {
            _context = context;
            Back_Command = new Command(BackToClients);
        }
        [ObservableProperty]
        private ObservableCollection<Client> _clients = new();
        
        [ObservableProperty]
        private Client _operatingClient = new ();//client that is currently eddited

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
                if((OperatingClient is null) || (filtered.First().client_key != OperatingClient.client_key))
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
            await ValidateClientAsync();
            if(Error == true)
            {
                Error = false;
                return;
            }
            await _context.AddItemAsync<Client>(OperatingClient);//create
            Clients.Add(OperatingClient);//add this client to the collection
            await Shell.Current.GoToAsync("//ClientsPage");

            //await _context.UpdateItemAsync<Client>(OperatingClient);//update

            //var clientCopy = OperatingClient.Clone();//copy

            //var index = Clients.IndexOf(OperatingClient);
            //Clients.RemoveAt(index);//remove it from the collection

            //Clients.Insert(index, clientCopy);
            SetOperatingClientCommand.Execute(new());//reset the value


        }

        [RelayCommand]
        private async Task UpdateClientAsync()
        {
            await ValidateClientAsync();
            if (Error == true)
            {
                Error = false;
                return;
            }
            await _context.UpdateItemAsync<Client>(OperatingClient);
            var clientCopy = OperatingClient.Clone();
            var index = Clients.IndexOf(OperatingClient);
            Clients.RemoveAt(index);
            Clients.Insert(index, clientCopy);

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
        private async void GoToClientIndiv(Client client)
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(Client)] = client,
                [nameof(Clients)] = Clients
            };
            await Shell.Current.GoToAsync("//ClientIndivPage", animate: true, parameter);
        }

        [RelayCommand]
        private async void GoToClientEdit(Client client)
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(Client)] = client,
                [nameof(Clients)] = Clients
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
