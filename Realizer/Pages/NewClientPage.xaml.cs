using CommunityToolkit.Maui.Views;
using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class NewClientPage : ContentPage
{
	private ClientsViewModel _viewModel;
	private PhoneNumViewModel _phoneNumViewModel;

    public NewClientPage(ClientsViewModel viewModel, PhoneNumViewModel phoneNumviewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _phoneNumViewModel = phoneNumviewModel;
        phoneEntry.BindingContext = _phoneNumViewModel;
    }

    private async void BackToClient_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ClientsPage");
    }


    

    //private async void AddedNewClient(object sender, EventArgs e)
    //{
    //       await _viewModel.SaveClientAsync();
    //       Navigation.PushAsync(new ClientsPage());

    //   }
}