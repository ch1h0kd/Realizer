
using Realizer.Models;
using Realizer.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Realizer.Pages;

public partial class NewVisitPage : ContentPage
{
	private readonly HistoryViewModel _viewModel;
	private readonly PurchaseViewModel _purchaseViewModel;
	private readonly ProductsViewModel _productsViewModel;
	private readonly ClientsViewModel _clientsViewModel;
    public NewVisitPage(HistoryViewModel viewModel, PurchaseViewModel purchaseViewModel, ProductsViewModel productsViewModel, ClientsViewModel clientsViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
		_purchaseViewModel = purchaseViewModel;
		_productsViewModel = productsViewModel;
		_clientsViewModel = clientsViewModel;
        NameSearchBar.SearchCommand = _clientsViewModel.SearchClientsCommand;
        ProductSearchBar.SearchCommand = _productsViewModel.SearchProductsCommand;

    }
    private async void BackHome_Clicked(Object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
	}

	private async void NextClicked(object sender, EventArgs e)
	{
        if(Validate("Client") == false)
        {
            //error client doesn't exist
            await Shell.Current.DisplayAlert("Alert", "Name does not exist", "Ok");
            return;
        }
        else // all good
        {
            NameEntry.IsVisible = false;
            NameSet.IsVisible = true;
            AddProductForm.IsVisible = true;
            DetailLabel.IsVisible = true;
        }
    }
    private void AddPurchaseClicked(object sender, EventArgs e)
    {
        //_viewModel.OperatingVisit.Purchases.Add(_purchaseViewModel.OperatingPurchase);//add to a list
        VisitsCollection.IsVisible=true;
        //add it to a list, if list not exist, make one


    }

    private async void SaveVisitAndBack(object sender, EventArgs e)
    {
        _viewModel.SaveVisitCommand.Execute(this);
        await Shell.Current.GoToAsync("//HistoryPage");

    }

    void NameSearchBar_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {   
        if (_viewModel.VisitedClient != null && e.NewTextValue != _viewModel.VisitedClient.client_name)
        {
            _viewModel.VisitedClient = null;
        }
        //select -> textChagne
        //visitedClient = null -> no hit
        //visitedClient = value -> valid
            //but it could change after getting the value
        NameSearchBar.SearchCommandParameter = e.NewTextValue;
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
       
        {
            _clientsViewModel.SearchClientsCommand.Execute(null);
            var results = _clientsViewModel.Clients;
            resultList.ItemsSource = results;
        }
    }
    void ProductSearchBar_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (_viewModel.PurchasedProduct != null && e.NewTextValue != _viewModel.PurchasedProduct.product_name)
        {
            _viewModel.PurchasedProduct = null;
        }
        ProductSearchBar.SearchCommandParameter = e.NewTextValue;
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))

        {
            _productsViewModel.SearchProductsCommand.Execute(null);
            var results = _productsViewModel.Products;
            resultList.ItemsSource = results;
        }
    }
    private async void FocusedChange(object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (NameSearchBar.IsFocused is true)//becomes focused
        {
            resultList.IsVisible = true;
            if (_clientsViewModel.Clients is null)
            {
                await _clientsViewModel.LoadClientsAsync();
            }
            resultList.ItemsSource = _clientsViewModel.Clients;
        }
        else//becomes unfocused
        {
            resultList.IsVisible = false;
            Console.WriteLine(NameSearchBar.Text);
        }
    }

    private async void ProdFocusedChange(object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (ProductSearchBar.IsFocused is true)//becomes focused
        {
            producteRsultList.IsVisible = true;
            if (_productsViewModel.Products is null)
            {
                await _productsViewModel.LoadProductsAsync();
            }
            producteRsultList.ItemsSource = _productsViewModel.Products;
        }
        else//becomes unfocused
        {
            producteRsultList.IsVisible = false;
        }
    }
    private async void NameSelected(object sender, SelectedItemChangedEventArgs args)
    {
        Client selectedClient = args.SelectedItem as Client;
        _viewModel.VisitedClient = selectedClient;
        NameSearchBar.Text = selectedClient.client_name;
        resultList.IsVisible = false;
    }
    private async void ProductSelected(object sender, SelectedItemChangedEventArgs args)
    {
        Product selectedProduct = args.SelectedItem as Product;
        _viewModel.PurchasedProduct = selectedProduct;
        ProductSearchBar.Text = selectedProduct.product_name;
        producteRsultList.IsVisible = false;
    }
    private bool Validate(string type)// check if these exist in the db
    {
        if (type == "Client")
        {
            return _viewModel.VisitedClient != null;
        }
        else if (type == "Product")
        {
            return _viewModel.PurchasedProduct != null;
        }
        return false;
    }

}