using Microsoft.Extensions.DependencyInjection;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Receiveds;
using NextFlix.Subscribers.Services;
using Serilog;

namespace NextFlix.Subscribers
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);


			Application.ThreadException += (s, e) => LogHelper.ExceptionLog(e.Exception);
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			{
				var ex = e.ExceptionObject as Exception;
				if (ex != null) LogHelper.ExceptionLog(ex);
			};


			var services = new ServiceCollection();
			services.AddSingleton<IRedisService, RedisService>();
			services.AddSingleton<CountryReceived>();
			services.AddSingleton<CategoryReceived>();
			services.AddSingleton<ChannelReceived>();
			services.AddSingleton<TagReceived>();
			services.AddSingleton<CastReceived>();
			services.AddSingleton<SourceReceived>();
			services.AddSingleton<UserReceived>();
			services.AddSingleton<Form1>(); 

			var serviceProvider = services.BuildServiceProvider();
			Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Information()
			.WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
			.CreateLogger();

			var form = serviceProvider.GetRequiredService<Form1>();
			Application.Run(form);
			Log.CloseAndFlush();

		}
	}
}