namespace MauiApp2;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(DataPage), typeof(DataPage));
        Routing.RegisterRoute(nameof(Formulaa), typeof(Formulaa));
        Routing.RegisterRoute(nameof(Ingredients), typeof(Ingredients));
        Routing.RegisterRoute(nameof(Batches), typeof(Batches));

    }
}
