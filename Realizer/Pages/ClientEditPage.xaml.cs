using System.Collections.ObjectModel;
using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

[QueryProperty(nameof(operatingClient), nameof(Client))] // receiving a client when moving to Indiv and Edit page
public partial class ClientEditPage : ContentPage
{
    private readonly ClientsViewModel _viewModel;
    private readonly PhoneNumViewModel _phoneNumviewModel;
    private ObservableCollection<PhoneNumber> numColl;

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

    public async void setItemsSource(int client_key)
    {
        await _phoneNumviewModel.LoadPhoneNumByIdAsync(client_key);//set operatingNums
        numColl = _phoneNumviewModel.OperatingNums;
        _viewModel.OperatingNums = numColl;//sync
        colView.ItemsSource = numColl;
        if(numColl.Count() > 1)
        {
            removeButton.IsVisible = true;
        }
        else _viewModel.OperatingNums.Add(new PhoneNumber());
    }

    void More_Clicked(System.Object sender, System.EventArgs e)
    {
        numColl.Add(new PhoneNumber());

        if (numColl.Count() == 2)
        {
           removeButton.IsVisible = true;
        }
    }

    async void Less_Clicked(System.Object sender, System.EventArgs e)
    {
        //delete the last phoneNumber
        var index = numColl.Count() - 1;
        await _phoneNumviewModel.DeletePhoneNumAsync(numColl[index].phoneNum_id);
        numColl.RemoveAt(index);

        if (numColl.Count() == 1)
        {
            removeButton.IsVisible = false;
        }
    }

}