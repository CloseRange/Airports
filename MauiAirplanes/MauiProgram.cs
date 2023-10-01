/*
 * Name:			Michael Hulbert
 * Description:		Lab 2
 * Date:			9/19/2023
 * Bugs:			N/A
 * Reflection:		Debugging a non-computer application is much harder. Getting the virtual machine working is even harder.
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
