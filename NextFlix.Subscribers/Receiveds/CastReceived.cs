using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class CastReceived(IRedisService redisService) : GenericRedisReceived<CastModel>(redisService, RedisPrefix.Cast)
	{
	}
}
