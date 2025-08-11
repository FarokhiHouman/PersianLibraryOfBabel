using System.Windows;

using Serilog;


namespace PersianLibraryOfBabel;
/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	protected override void OnStartup(StartupEventArgs e) {
		base.OnStartup(e);
		Log.Logger = new LoggerConfiguration().WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day).
											   CreateLogger();
		Log.Information("App started at {Time}", DateTime.Now);
	}
}