using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class UserReceived(IRedisService redisService) : GenericRedisReceived<UserModel>(redisService, RedisPrefix.User)
	{
	}
}
