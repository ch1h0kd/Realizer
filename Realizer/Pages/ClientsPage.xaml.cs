using Realizer.Data;
using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class ClientsPage : ContentPage
{
    private readonly ClientsViewModel _viewModel;
    public ClientsPage(ClientsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    public ClientsViewModel get_viewModel() { return _viewModel; }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        //Shell.SetTabBarIsVisible(this, true);
        await _viewModel.LoadClientsAsync();
    }

    private async void AddClient_Clicked(object sender, EventArgs e)
    {
        var parameter = new Dictionary<string, object>
        {
            [nameof(ClientsViewModel.Clients)] = _viewModel.Clients
        };
        await Shell.Current.GoToAsync("//NewClientPage", animate: true, parameter);
    }

    void searchBar_changed(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _viewModel.SearchClientsCommand.Execute(null);
        }
    }
}