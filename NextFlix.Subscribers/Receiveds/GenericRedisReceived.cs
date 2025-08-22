using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using System.Text.Json;

namespace NextFlix.Subscribers.Receiveds
{
	public class GenericRedisReceived<T>(IRedisService redisService,RedisPrefix prefix)
		where T:class, IId,new()
	{
		public async Task<bool> Received(string message, RoutingKey routingKey)
		{
			switch (routingKey)
			{
				case RoutingKey.Created:
				case RoutingKey.Updated:
					return await Upsert(message);
				case RoutingKey.Deleted:
					return await Delete(message);
				default:
					break;
			}
			return false;
		}

		public virtual async Task<bool> Upsert(string message)
		{
			try
			{
				T model = JsonSerializer.Deserialize<T>(message);
				bool result = false;
				result = await redisService.StringSetAsync($"{prefix}:{model.Id}", model);
				if (result)
				{
					result = await redisService.HashSetAsync($"{prefix}", model.Id, model);
				}
				return result;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}

		}

		public virtual async Task<bool> Delete(string message)
		{
			try
			{
				List<int>? ids = JsonSerializer.Deserialize<List<int>>(message);
				bool result = false;
				if (ids is null)
					return true;
				List<string> keys = ids.Select(id => $"{prefix}:{id}").ToList();
				result = await redisService.StringDeleteAsync(keys);
				if (result)
				{
					result = await redisService.HashDeleteAsync($"{prefix}", ids);
				}
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
