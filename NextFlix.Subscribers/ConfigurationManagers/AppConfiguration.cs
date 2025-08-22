using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextFlix.Subscribers.ConfigurationManagers
{
	static class AppConfiguration
	{
		private static IConfigurationRoot configuration;

		static AppConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

			configuration = builder.Build();
		}

		public static string GetValue(string key)
		{
			return configuration[key];
		}

		public static T GetSection<T>(string sectionName)
		{
			return configuration.GetSection(sectionName).Get<T>();
		}
	}
}
