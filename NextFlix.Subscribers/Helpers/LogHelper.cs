using Serilog;
using System.Text;

namespace NextFlix.Subscribers.Helpers
{
	public static class LogHelper
	{

		private static void AppendLogHeader(StringBuilder sb)
		{
			sb.AppendLine();
			sb.AppendLine("<----------------------------------------------");
			sb.AppendLine($"Tarih: {DateTime.Now}");
		}

		private static void AppendLogFooter(StringBuilder sb)
		{
			sb.AppendLine("---------------------------------------------->");
			sb.AppendLine();
		}
		public static void ExceptionLog(Exception ex)
		{
			string message = ex.Message;
			string stackTrace = ex.StackTrace ?? string.Empty;
			StringBuilder sb = new();
			AppendLogHeader(sb);
			sb.AppendLine($"Hata Mesajı: {message}");
			sb.AppendLine($"Stack Trace: {stackTrace}");
			AppendLogFooter(sb);

			Log.Error(sb.ToString());
		}

		public static void RabbitMQLog(string queueName,string routingKey,string message,bool success) 
		{
			StringBuilder sb = new();
			AppendLogHeader(sb);
			if(success)
				sb.AppendLine("RabbitMQ İşlemi Başarılı");
			else
				sb.AppendLine("RabbitMQ İşlemi Başarısız");
			sb.AppendLine($"Queue Name: {queueName}");
			sb.AppendLine($"Routing Key: {routingKey}");
			sb.AppendLine($"Message: {message}");
			AppendLogFooter(sb);
			Log.Information(sb.ToString());
		}
	}
}
