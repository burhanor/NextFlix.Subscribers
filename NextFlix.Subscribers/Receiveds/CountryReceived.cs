using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class CountryReceived(IRedisService redisService) : GenericRedisReceived<CountryModel>(redisService, RedisPrefix.Country)
	{
		
	}
}
