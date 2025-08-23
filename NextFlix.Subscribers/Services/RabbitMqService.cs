using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NextFlix.Subscribers.Services
{
	
	public class RabbitMqService:IRabbitMqService
	{
		private readonly IConnection _connection;


		private  IChannel? _channel;

		private bool _connected;
		BasicProperties properties = new BasicProperties()
		{
			DeliveryMode = DeliveryModes.Persistent
		};
		public RabbitMqService(IConnection connection)
		{
			_connection = connection;

		}

		public async Task Connect(CancellationToken cancellationToken)
		{
			if (_connected)
				return;
			try
			{
				if (_connected)
					return;

				_channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

				_connected = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"RabbitMQ connection error: {ex.Message}");
				throw;
			}
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


		public async Task Publish(RabbitMqQueues exchange, RoutingKey routingType, object message, CancellationToken cancellationToken)
		{
			string exchangeName = exchange.ToString();
			string routingKey = routingType.ToString();
			string queueName = exchange.ToString();

			if (!_connected || _channel is null)
			{
				await Connect(cancellationToken);
			}

			try
			{
				

				var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));



				await _channel.BasicPublishAsync(
					exchange: exchangeName,
					routingKey: routingKey,
					mandatory: false,
					basicProperties: properties,
					body: body,
					cancellationToken: cancellationToken
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Publish error: {ex.Message}");
				throw;
			}
		}

	}
}
