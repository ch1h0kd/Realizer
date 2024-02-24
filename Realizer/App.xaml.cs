using Realizer.Pages;
using Realizer.Services;
using Microsoft.Maui.Controls.Hosting;

namespace Realizer
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf0x1RXxbf1x0ZF1MYlhbR3NPIiBoS35RdURhWn5ccHZRRWZeVEZ1\r\n");
            InitializeComponent();
            MainPage = new AppShell();

        }
    }
}
