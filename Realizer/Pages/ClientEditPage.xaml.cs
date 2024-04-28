using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

[QueryProperty(nameof(operatingClient), nameof(Client))] // receiving a client when moving to Indiv and Edit page
public partial class ClientEditPage : ContentPage
{
    private readonly ClientsViewModel _viewModel;
    private readonly PhoneNumViewModel _phoneNumviewModel;

    Client client;
    public Client operatingClient
    {
        get => client;
        set
        {
            client = value;
            setItemsSource(client.client_key);
            OnPropertyChanged();
        }
    }

    public ClientEditPage(ClientsViewModel viewModel, PhoneNumViewModel phoneNumviewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _phoneNumviewModel = phoneNumviewModel;
    }

    public async void setItemsSource(int key)
    {
        await _phoneNumviewModel.LoadPhoneNumByIdAsync(key);//set operatingNums
        _viewModel.OperatingNums = _phoneNumviewModel.OperatingNums;//sync
        colView.ItemsSource = _phoneNumviewModel.OperatingNums;

    }
    //private void updateClicked(object sender, EventArgs e)
    //{
    //    _viewModel.UpdateClientCommand.Execute(this);
    //    _viewModel.GoToClientIndivCommand.Execute(this);
    //    //show saved or not
    //}

    //private async void BackToClient_Clicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync("/ClientIndivPage");
    //}
}