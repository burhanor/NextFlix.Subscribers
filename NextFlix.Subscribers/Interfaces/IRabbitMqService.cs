using NextFlix.Subscribers.Enums;

namespace NextFlix.Subscribers.Interfaces
{
	public interface IRabbitMqService
	{
		Task Subscribe(string queueName, Func<string, RoutingKey, Task<bool>> onMessageReceived);
		Task Publish(RabbitMqQueues exchange, RoutingKey routingType, object message, CancellationToken cancellationToken);
	}
}
