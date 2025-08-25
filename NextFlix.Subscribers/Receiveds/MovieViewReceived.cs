using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;
using System.Text.Json;

namespace NextFlix.Subscribers.Receiveds
{
	public class MovieViewReceived(IRedisService redisService) : GenericRedisReceived<IdModel>(redisService, RedisPrefix.MovieView)
	{
		RedisPrefix prefix = RedisPrefix.MovieView;
		public override async Task<bool> Upsert(string message)
		{
			try
			{
				int id = JsonSerializer.Deserialize<int>(message);
				bool result = await redisService.HashSetAsync($"{prefix}", id, id);
				return result;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}

		}
	}
}
