using Realizer.ViewModels;

namespace Realizer.Pages;

public partial class NewProductPage : ContentPage
{
	private ProductsViewModel _viewModel;
	public NewProductPage(ProductsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
		_viewModel=viewModel;
	}
	private async void BackToProduct_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//ProductsPage");
	}
	//private void isAddingNew()
	//{
	//	var picker = (Picker)sender
	//	int selectedIndex = picker.SelectedIndex;

	//	if(selectedIndex == -1)
	//	{
	//		//display enter box to add a new category
	//	}
	//}
	private void AddCategory(string newCategory)
	{
		Picker picker = new Picker { Title = "Select a Category" };
		picker.Items.Add(newCategory);
	}
}