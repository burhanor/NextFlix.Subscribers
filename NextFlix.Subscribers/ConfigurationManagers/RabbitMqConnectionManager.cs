using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Models.RabbitMQ;
using RabbitMQ.Client;

namespace NextFlix.Subscribers.ConfigurationManagers
{
	public class RabbitMqConnectionManager : IDisposable
	{
		private readonly ConnectionFactory _factory;
		private IConnection _connection;
		private readonly RabbitMQModel rabbitMQModel = AppConfiguration.GetSection<RabbitMQModel>("RabbitMQ");	
		public RabbitMqConnectionManager()
		{

			_factory = new ConnectionFactory()
			{
				HostName = rabbitMQModel.Host,
				UserName = rabbitMQModel.Username,
				Password = rabbitMQModel.Password,
				VirtualHost = rabbitMQModel.VirtualHost,
				AutomaticRecoveryEnabled = true, 
				NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
			};
		}

		public async Task<IConnection> GetConnection()
		{
			try
			{
				if (_connection == null || !_connection.IsOpen)
				{
					_connection = await _factory.CreateConnectionAsync();
				}
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
			}
			
			return _connection;
		}

		void IDisposable.Dispose()
		{
			if (_connection != null)
			{
				_connection.CloseAsync().GetAwaiter().GetResult();
				_connection.Dispose();
			}
		}
	}
}
