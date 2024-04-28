using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

//[(name of the property in the page that will receive the data, query parameter name that corresponds to the property)]
[QueryProperty(nameof(operatingClient), nameof(Client))] // receiving a client when moving to Indiv and Edit page
public partial class ClientIndivPage : ContentPage
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

    public ClientIndivPage() { }
    public ClientIndivPage(ClientsViewModel viewModel, PhoneNumViewModel phoneNumviewModel)
    //public ClientIndivPage(ClientIndivViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _phoneNumviewModel = phoneNumviewModel;
    }

    public async void setItemsSource(int key)
    {
        await _phoneNumviewModel.LoadPhoneNumByIdAsync(key);//set operatingNums
        colView.ItemsSource = _phoneNumviewModel.OperatingNums;

    }
}
