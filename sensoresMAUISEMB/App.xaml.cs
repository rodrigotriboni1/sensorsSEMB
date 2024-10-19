namespace sensoresMAUISEMB
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            DependencyService.Register<CPUInfoImplementation>();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");

        }
    }
}
