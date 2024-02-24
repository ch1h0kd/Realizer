using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class ClientEditPage : ContentPage
{
    private readonly ClientsViewModel _viewModel;
	public ClientEditPage(ClientsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
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