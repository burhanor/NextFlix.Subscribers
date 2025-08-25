using Hangfire;
using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Services
{
	public class HangfireService
	{
		private readonly string _connectionString = AppConfiguration.GetValue("ConnectionStrings:HangfireDbConnection");

		private readonly IRedisService _redisService;
		private readonly IRabbitMqService _rabbitMqService;
		private readonly BackgroundJobServer _hangfireServer;
		
		public HangfireService(IRedisService redisService,IRabbitMqService rabbitMqService)
		{
			_redisService = redisService;
			_rabbitMqService = rabbitMqService;
			GlobalConfiguration.Configuration.UseSqlServerStorage(_connectionString);
			_hangfireServer = new BackgroundJobServer();
			RecurringJob.AddOrUpdate(
				"UpdateMeiliSearch_MovieView",
				() => HangfireJobs.UpdateMeiliSearchAsync(RedisPrefix.MovieView.ToString()),
				"*/10 * * * *" // her 10 dakikada bir
			);

			RecurringJob.AddOrUpdate(
				"UpdateMeiliSearch_MovieLike",
				() => HangfireJobs.UpdateMeiliSearchAsync(RedisPrefix.MovieLike.ToString()),
				"*/10 * * * *" // her 10 dakikada bir
			);
		}
		

		
	}
}
