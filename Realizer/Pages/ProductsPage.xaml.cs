using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class ProductsPage : ContentPage
{
	private readonly ProductsViewModel _viewModel;
	public ProductsPage(ProductsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        //Shell.SetTabBarIsVisible(this, true);
        await _viewModel.LoadProductsAsync();
    }
    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//NewProductPage");
    }

    void searchBar_changed(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _viewModel.SearchProductsCommand.Execute(null);
        }
    }
}