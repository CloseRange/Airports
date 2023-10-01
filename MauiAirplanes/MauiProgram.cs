/*
 * Name:			Michael Hulbert
 * Description:		Lab 3
 * Date:			9/30/2023
 * Bugs:			Cockroach
 * Reflection:		Easier than other 2 labs. Was able to get it done in a few hours
 */
using Microsoft.Extensions.Logging;

namespace MauiAirplanes;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
