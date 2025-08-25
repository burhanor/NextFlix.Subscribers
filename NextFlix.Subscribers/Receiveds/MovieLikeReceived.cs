using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;
using NextFlix.Subscribers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NextFlix.Subscribers.Receiveds
{
	public class MovieLikeReceived(IRedisService redisService) : GenericRedisReceived<IdModel>(redisService, RedisPrefix.MovieLike)
	{
		RedisPrefix prefix = RedisPrefix.MovieLike;
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
