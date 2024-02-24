using Realizer.Models;
using Realizer.ViewModels;
using Syncfusion.Maui.Core.Carousel;

namespace Realizer.Pages;

public partial class MainPage : ContentPage
{
    //public MainPage()
    //{
    //    InitializeComponent();
    //    BindingContext = new MainPageViewModel();
    //}

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void GoToNewVisit(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//NewVisitPage");
    }

}
