using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Data;
using Realizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Realizer.ViewModels
{
    public partial class PurchaseViewModel : ObservableObject
    {
        public readonly DatabaseContext _context;//instance from DatabaseContext
        public ICommand Back_Command { get; set; }
        public PurchaseViewModel(DatabaseContext context)
        {
            _context = context;
            //Back_Command = new Command(BackToPurchases);
        }

        [ObservableProperty]
        private ObservableCollection<Purchase> _purchases = new();

        [ObservableProperty]
        private Purchase _operatingPurchase = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        public async Task LoadPurchasesAsync()
        {
            await ExecuteAsync(async () =>
            {
                var purchases = await _context.GetAllAsync<Purchase>();
                //var products = await _context.GetAllAsync<Product>();

                if (purchases is not null && purchases.Any())//if we have at least one purchase
                {
                    Purchases ??= new ObservableCollection<Purchase>(); //if null, initialize

                    foreach (var purchase in purchases)//insert each purchase into a obervable collection Purchases 
                    {
                        Purchases.Add(purchase);
                    }
                }
            }, "Fetching purchases...");
        }

        [RelayCommand]
        private void SetOperatingPurchase(Purchase? purchase) => OperatingPurchase = purchase ?? new();

        [RelayCommand]
        private async Task SavePurchaseAsync()//loading
        {
            if (OperatingPurchase is null)//do nothing
                return;
            var busyText = OperatingPurchase.purchase_id == 0 ? "Creating purchase..." : "Updating purchase";
            await ExecuteAsync(async () =>
            {
                //if (OperatingPurchase.Id == 0)//purchase doesn't exist, create a new one//if 0, show message: "set ID"
                //{
                await _context.AddItemAsync<Purchase>(OperatingPurchase);//create
                Purchases.Add(OperatingPurchase);//add this purchase to the collection
                //}
                //else
                //{
                //    await _context.UpdateItemAsync<Purchase>(OperatingPurchase);//update

                //    var clientCopy = OperatingPurchase.Clone();//copy

                //    var index = Purchases.IndexOf(OperatingPurchase);
                //    Purchases.RemoveAt(index);//remove it from the collection

                //    Purchases.Insert(index, clientCopy);

                //}
                SetOperatingPurchaseCommand.Execute(new());//reset the value
            }, busyText);

        }

        [RelayCommand]
        private async Task DeletePurchaseAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeteleItemByKeyAsync<Purchase>(id))
                {
                    var purchase = Purchases.FirstOrDefault(p => p.purchase_id == id);//get the item
                    Purchases.Remove(purchase);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Purchase was not deleted", "Ok");
                }
            }, "Deleting purchase...");
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

    }
}
