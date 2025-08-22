using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;

namespace NextFlix.Subscribers.Receiveds
{
	public class CategoryReceived(IRedisService redisService) : GenericRedisReceived<CategoryModel>(redisService, RedisPrefix.Category)
	{
	}
}
