using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Data;
using Realizer.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Realizer.ViewModels
{
    public partial class HistoryViewModel : ObservableObject
    {
        public readonly DatabaseContext _context;//instance from DatabaseContext
        public ICommand Back_Command { get; set; }
        public HistoryViewModel(DatabaseContext context)
        {
            _context = context;
            //Back_Command = new Command(BackToVisits);
        }

        [ObservableProperty]
        private ObservableCollection<Visit> _visits = new();

        [ObservableProperty]
        public Visit _operatingVisit = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        [ObservableProperty]
        private string _status;

        [ObservableProperty]
        private DateTime _maxDate = DateTime.Today;

        [ObservableProperty]
        private Client _visitedClient;

        [ObservableProperty]
        private Product _purchasedProduct;
        public async Task LoadVisitsAsync()
        {
            await ExecuteAsync(async () =>
            {
                var visits = await _context.GetAllAsync<Visit>();
                //var products = await _context.GetAllAsync<Product>();

                if (visits is not null && visits.Any())//if we have at least one visit
                {
                    Visits ??= new ObservableCollection<Visit>(); //if null, initialize

                    foreach (var visit in visits)//insert each visit into a obervable collection Visits 
                    {
                        Visits.Add(visit);
                    }
                }
            }, "Fetching visits...");
        }

        [RelayCommand]
        private void SetOperatingVisit(Visit? visit) => OperatingVisit = visit ?? new();


        [RelayCommand]
        private async Task SaveVisitAsync()//loading
        {
            _status = "";
            if (OperatingVisit is null)
            {//do nothing
                return;
            }
            if (!string.IsNullOrWhiteSpace(OperatingVisit.note) && OperatingVisit.visit_id != 0)
            {
                var busyText = _context.GetFilteredAsync<Visit>(x => x.visit_id == OperatingVisit.visit_id) is null ? "New ID" : "ID exists";
                await ExecuteAsync(async () =>
                {
                    if (busyText == "New ID")//client doesn't exist, create a new one
                    {
                        await _context.AddItemAsync<Visit>(OperatingVisit);//create
                        _status = "success";
                        Visits.Add(OperatingVisit);//add this client to the collection
                    }
                    else
                    {
                        _status = "ID duplicate";
                    }

                    //await _context.UpdateItemAsync<Client>(OperatingClient);//update

                    //var clientCopy = OperatingClient.Clone();//copy

                    //var index = Clients.IndexOf(OperatingClient);
                    //Clients.RemoveAt(index);//remove it from the collection

                    //Clients.Insert(index, clientCopy);
                    SetOperatingVisitCommand.Execute(new());//reset the value
                }, busyText);
            }
            else _status = ("Missing info");


        }
        //{
        //    if (OperatingVisit is null)//do nothing
        //        return;
        //    var busyText = OperatingVisit.Id == 0 ? "Creating visit..." : "Updating visit";
        //    await ExecuteAsync(async () =>
        //    {
        //        //if (OperatingVisit.Id == 0)//visit doesn't exist, create a new one//if 0, show message: "set ID"
        //        //{
        //        await _context.AddItemAsync<Visit>(OperatingVisit);//create
        //        Visits.Add(OperatingVisit);//add this visit to the collection
        //        //}
        //        //else
        //        //{
        //        //    await _context.UpdateItemAsync<Visit>(OperatingVisit);//update

        //        //    var clientCopy = OperatingVisit.Clone();//copy

        //        //    var index = Visits.IndexOf(OperatingVisit);
        //        //    Visits.RemoveAt(index);//remove it from the collection

        //        //    Visits.Insert(index, clientCopy);

        //        //}
        //        SetOperatingVisitCommand.Execute(new());//reset the value
        //    }, busyText);

        //}

        [RelayCommand]
        private async Task DeleteVisitAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeteleItemByKeyAsync<Visit>(id))
                {
                    var visit = Visits.FirstOrDefault(p => p.visit_id == id);//get the item
                    Visits.Remove(visit);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Visit was not deleted", "Ok");
                }
            }, "Deleting visit...");
        }

        //if not string provided, display default message which is before and after the operation given
  
        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                Console.WriteLine("here");
                await operation?.Invoke();
            }
            finally
            {
                IsBusy = false;//after the operations, not busy anymore
                BusyText = "Processing...";
            }
        }
        
        [RelayCommand]
        private async Task DateTimeToString(DateTime selectedDate)
        {
            OperatingVisit.date = selectedDate.ToString();
        }
    }
}
