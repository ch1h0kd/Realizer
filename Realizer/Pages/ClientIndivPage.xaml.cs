using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

//[QueryProperty(nameof(EdittedClient), "edittedClient")]
public partial class ClientIndivPage : ContentPage
{
    private readonly ClientsViewModel _viewModel;

    public ClientIndivPage() {}
    public ClientIndivPage(ClientsViewModel viewModel)
    //public ClientIndivPage(ClientIndivViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        //_viewModel.OperatingClient = EdittedClient;
        //Console.WriteLine(_viewModel.Client);
    }

    private async void GoToEditPage(object sender, EventArgs e)
    {
        
    }
}