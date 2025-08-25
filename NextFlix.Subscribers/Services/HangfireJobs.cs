using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Services
{
	public static class HangfireJobs
	{
		private static readonly RedisService redisService = new RedisService();
		private static readonly RabbitMqConnectionManager rabbitMqConnectionManager = new RabbitMqConnectionManager();
		private static readonly RabbitMqService rabbitMqService = new RabbitMqService(rabbitMqConnectionManager.GetConnection().Result);

		public static async Task UpdateMeiliSearchAsync(string prefix)
		{
			var entries = await redisService.HashGetAsync(prefix);
			if (entries?.Count > 0)
			{
				foreach (var movieId in entries.Distinct().ToList())
				{
					await rabbitMqService.Publish(RabbitMqQueues.Movies, RoutingKey.Updated, new IdModel
					{
						Id = movieId
					}, CancellationToken.None);
				}
				await redisService.HashDeleteAsync(prefix, entries);
			}
		}
	}
}
