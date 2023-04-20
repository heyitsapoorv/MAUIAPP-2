using Microsoft.Extensions.Logging;
namespace MauiApp2;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.DependencyInjection;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
        
        builder.Services.AddSingleton<BLEService>();
        builder.Services.AddSingleton<Dbservice>();
        //builder.Services.AddSingleton(new Dbservice());


        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);

        builder.Services.AddSingleton<Home>();
        builder.Services.AddSingleton<HomePage>();

        builder.Services.AddSingleton<Data>();
        builder.Services.AddSingleton<DataPage>();
        builder.Services.AddSingleton<Formulas>();
        builder.Services.AddSingleton<Formulaa>();
        builder.Services.AddSingleton<Ingredients_vm>();
        builder.Services.AddSingleton<Ingredients>();
        builder.Services.AddSingleton<Batches>();
        builder.Services.AddSingleton<Batch>();

#endif

        return builder.Build();
	}
}
