using Realizer.Models;
using Realizer.ViewModels;
using System.Collections.ObjectModel;

namespace Realizer.Pages;

public partial class HistoryPage : ContentPage
{
	private readonly HistoryViewModel _viewModel;
	public ObservableCollection<VisitDateGroup> Visits { get; set; } = new ObservableCollection<VisitDateGroup>();//a whole page, a list of dates
	public HistoryPage() { }
	public HistoryPage(HistoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}

	private async void AddVisit_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//NewVisitPage");
	}
}