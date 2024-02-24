using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realizer.Data;
using Realizer.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;
        public ProductsViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        private Product _operatingProduct = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        [ObservableProperty]
        private bool _searching;

        public async Task LoadProductsAsync()
        {
            await ExecuteAsync(async () =>
            {
                var products = await _context.GetAllAsync<Product>();
                if (products != null && products.Any())
                {
                    Products ??= new ObservableCollection<Product>();
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }
            }, "Fetching products");
        }


        [RelayCommand]
        private async Task GetListOfNames(string type)
        {
            await ExecuteAsync(async () =>
            {
                var productsList = await _context.GetAllAsync<Product>();
                if (productsList != null && productsList.Any())
                {
                    ObservableCollection<string> LstName = new ObservableCollection<string>();
                    foreach (var product in productsList)
                    {
                        LstName.Add(product.product_name);
                    }
                }

            }, "Fetching products");
        }

        [RelayCommand]
        private void SetOperatingProduct(Product? product) => OperatingProduct = product ?? new();

        [RelayCommand]
        private async Task SaveProductAsync()//loading
        {
            if (OperatingProduct is null)//do nothing
                return;
            var (isValid, errorMessage) = OperatingProduct.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Alert", errorMessage, "Ok");
                return;
            }

            var filtered = await _context.GetFilteredAsync<Product>(x => x.product_name == OperatingProduct.product_name);
            if (filtered is not null && filtered.Any())//if we have at least one product name match
            {
                await Shell.Current.DisplayAlert("Alert", "This product already exists", "Ok");
                return;
            }
            await _context.AddItemAsync<Product>(OperatingProduct);//create
            Products.Add(OperatingProduct);//add this client to the collection
            await Shell.Current.GoToAsync("//ProductsPage");
            
            //    await _context.UpdateItemAsync<Client>(OperatingClient);//update

            //    var clientCopy = OperatingClient.Clone();//copy

            //    var index = Clients.IndexOf(OperatingClient);
            //    Clients.RemoveAt(index);//remove it from the collection

            //    Clients.Insert(index, clientCopy);

            //}
            SetOperatingProductCommand.Execute(new());//reset the value
        }

        [RelayCommand]
        private async Task DeleteProductAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeteleItemByKeyAsync<Product>(id))
                {
                    var product = Products.FirstOrDefault(p => p.product_id == id);//get the item
                    Products.Remove(product);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Product was not deleted", "Ok");
                }
            }, "Deleting product...");
        }

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
        private async void GoToProductIndiv(Product product)
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(ProductIndivViewModel.Product)] = product
            };
            await Shell.Current.GoToAsync("//ProductIndivPage", animate: true, parameter);
        }

        [RelayCommand]
        private async Task SearchProducts(string searchTerm)
        {
            Products.Clear();
            Searching = true;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                await LoadProductsAsync();
                return;
            }
            foreach (var product in await _context.GetFilteredAsync<Product>(x => x.product_name.Contains(searchTerm)))
            {
                Products.Add(product);
            }
            Searching = false;
        }

        [RelayCommand]
        private async void deleteall()
        {
            var products = await _context.GetAllAsync<Product>();
            foreach (var product in products)
            {
                if (await _context.DeteleItemByKeyAsync<Product>(product.product_id))
                {
                    //var client = Clients.FirstOrDefault(p => p.client_id == id);//get the item
                    Products.Remove(product);
                }
            }
        }
    }
}
