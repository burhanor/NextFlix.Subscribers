using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class TagReceived(IRedisService redisService) : GenericRedisReceived<TagModel>(redisService, RedisPrefix.Tag)
	{
		
	}
}
