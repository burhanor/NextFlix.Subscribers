using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class ChannelReceived(IRedisService redisService) : GenericRedisReceived<ChannelModel>(redisService, RedisPrefix.Channel)
	{
	}
}
