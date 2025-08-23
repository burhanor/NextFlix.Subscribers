using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;
using NextFlix.Subscribers.Services;
using System.Text.Json;

namespace NextFlix.Subscribers.Receiveds
{
	public class CastReceived(IRedisService redisService,MovieService movieService,IRabbitMqService rabbitMqService) : GenericRedisReceived<CastModel>(redisService, RedisPrefix.Cast)
	{
		public override async Task<bool> Upsert(string message)
		{
			CastModel? model = JsonSerializer.Deserialize<CastModel>(message);
			if (model is null)
				return false;
			
			List<int> movieIds = await movieService.GetMovieIdFromCastId(model.Id);
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
