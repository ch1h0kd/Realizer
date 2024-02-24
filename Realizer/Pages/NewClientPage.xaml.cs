using CommunityToolkit.Maui.Views;
using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class NewClientPage : ContentPage
{
	private ClientsViewModel _viewModel;

    public NewClientPage(ClientsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

    }

    private async void BackToClient_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ClientsPage");
    }


    ////DatePicker datePicker = new DatePicker
    ////{
    ////    MaximumDate = DateTime.Today
    ////};
    //DatePicker datepicker = datepick

    //private async void AddedNewClient(object sender, EventArgs e)
    //{
    //       await _viewModel.SaveClientAsync();
    //       Navigation.PushAsync(new ClientsPage());

    //   }
}