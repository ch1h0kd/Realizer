using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class ProductIndivPage : ContentPage
{
    private readonly ProductIndivViewModel _viewModel;

    public ProductIndivPage() { }
    public ProductIndivPage(ProductIndivViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

	private async void GoToEditPage(Object sender,  EventArgs e)
	{

	}
}