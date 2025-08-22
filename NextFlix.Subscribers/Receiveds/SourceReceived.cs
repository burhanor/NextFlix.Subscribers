using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class SourceReceived(IRedisService redisService) : GenericRedisReceived<SourceModel>(redisService, RedisPrefix.Source)
	{
	}
}
