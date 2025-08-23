using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;
using NextFlix.Subscribers.Services;
using System.Text.Json;

namespace NextFlix.Subscribers.Receiveds
{
	public class UserReceived(IRedisService redisService, IRabbitMqService rabbitMqService, MovieService movieService) : GenericRedisReceived<UserModel>(redisService, RedisPrefix.User)
	{
		public override async Task<bool> Upsert(string message)
		{
			UserModel? model = JsonSerializer.Deserialize<UserModel>(message);
			if (model is null)
				return false;

			List<int> movieIds = await movieService.GetMovieIdFromUserId(model.Id);
			if (movieIds?.Count > 0)
			{
				foreach (var id in movieIds)
				{
					await rabbitMqService.Publish(RabbitMqQueues.Movies, RoutingKey.Updated, new IdModel { Id = id }, CancellationToken.None);
				}
			}

			return await base.Upsert(message);
		}
	}
}
