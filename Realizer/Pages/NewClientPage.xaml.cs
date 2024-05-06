using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Realizer.Models;
using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class NewClientPage : ContentPage
{
    private ClientsViewModel _viewModel;
    private ObservableCollection<PhoneNumber> numColl;
    //private PhoneNumViewModel _phoneNumViewModel;

    public NewClientPage(ClientsViewModel viewModel) //, PhoneNumViewModel phoneNumviewModel
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        numColl = _viewModel.OperatingNums;
        numColl.Add(new Models.PhoneNumber());
    }

    private async void BackToClient_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ClientsPage");
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
        numColl.RemoveAt(index);

        if (numColl.Count() == 1)
        {
            removeButton.IsVisible = false;
        }
    }
}