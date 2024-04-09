using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

//[QueryProperty(nameof(EdittedClient), "edittedClient")]
public partial class ClientIndivPage : ContentPage
{
    private readonly ClientsViewModel _viewModel;
    private readonly PhoneNumViewModel _phoeNumviewModel;

    public ClientIndivPage() {}
    public ClientIndivPage(ClientsViewModel viewModel, PhoneNumViewModel phoneNumviewModel)
    //public ClientIndivPage(ClientIndivViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _phoeNumviewModel = phoneNumviewModel;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        //get the client's phone numbers
        await _phoeNumviewModel.LoadPhoneNumByIdAsync(_phoeNumviewModel.OperatingClientId);//set operatingNums
        colView.ItemsSource = _phoeNumviewModel.OperatingNums;
    }
}