using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NextFlix.Subscribers.Services
{
	
	public class RabbitMqService
	{
		private readonly IConnection _connection;


		public RabbitMqService(IConnection connection)
		{
			_connection = connection;
		}

		public async Task Subscribe(string queueName, Func<string,RoutingKey, Task<bool>> onMessageReceived)
		{
			try
			{
				var channel = await _connection.CreateChannelAsync();

				await channel.QueueDeclareAsync(queue: queueName,
									 durable: true,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				var consumer = new AsyncEventingBasicConsumer(channel);

				consumer.ReceivedAsync += async (model, ea) =>
				{
					var message = Encoding.UTF8.GetString(ea.Body.ToArray());
					bool isSuccess = false;
					try
					{
						RoutingKey key = (RoutingKey)Enum.Parse(typeof(RoutingKey), ea.RoutingKey);
						isSuccess = await onMessageReceived(message, key);

						LogHelper.RabbitMQLog(queueName, ea.RoutingKey, message,isSuccess);

					}
					catch (Exception ex)
					{
						LogHelper.ExceptionLog(ex);
						isSuccess = false;
					}

					if (isSuccess)
						await channel.BasicAckAsync(ea.DeliveryTag, false);
					else
						await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);

					
				};

				await channel.BasicQosAsync(0, 1, false);
				await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
			}
			
		}
	}
}
