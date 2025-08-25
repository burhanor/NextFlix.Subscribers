using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models.Redis;
using Serilog;
using StackExchange.Redis;
using System.Text.Json;

namespace NextFlix.Subscribers.Services
{
	public class RedisService:IRedisService
	{
		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _db;
		private bool _isConnected;

		public bool IsConnected
		{
			get => _isConnected;
			set
			{
				if (_isConnected != value)
				{
					_isConnected = value;
					OnConnectionStatusChanged?.Invoke(this, _isConnected);
				}
			}
		}
		public event EventHandler<bool> OnConnectionStatusChanged;

		private readonly RedisModel redisModel = AppConfiguration.GetSection<RedisModel>("Redis");
		public RedisService()
		{
			IsConnected = false;
			var config = new ConfigurationOptions
			{
				EndPoints = { $"{redisModel.Host}:{redisModel.Port}" },
				Password = redisModel.Password
			};

			const int delayMilliseconds = 2000;
			while (true)
			{
				try
				{
					Log.Information($"Redis'e bağlanılıyor: {redisModel.Host}:{redisModel.Port}");
					_redis = ConnectionMultiplexer.Connect(config);
					IsConnected = _redis.IsConnected;
					break;
				}
				catch (Exception ex)
				{
					LogHelper.ExceptionLog(ex);
					Thread.Sleep(delayMilliseconds);
				}
			}
			Log.Information($"Redis'e bağlandı: {redisModel.Host}:{redisModel.Port}");

			_redis.ConnectionFailed += (sender, e) =>
			{
				IsConnected = false;
				Log.Information("Redis", "", $"Bağlantı koptu: {e.Exception?.Message}", false);
			};

			_redis.ConnectionRestored += (sender, e) =>
			{
				IsConnected = true;
				Log.Information("Redis", "", "Bağlantı geri geldi", true);
			};

			_redis.ErrorMessage += (sender, e) =>
			{
				LogHelper.ExceptionLog(new Exception($"Redis hata mesajı: {e.Message}"));
			};

			_db = _redis.GetDatabase();
		}


		public async Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null)
		{
			try
			{

				var json = JsonSerializer.Serialize(value);
				return await _db.StringSetAsync(key, json, expiry);
				
			}
			catch (Exception ex)
			{

				LogHelper.ExceptionLog(ex);
				return false;
			}
		}

		public async Task<bool> HashSetAsync<T>(string hashKey, int id, T value, TimeSpan? expiry = null)
		{
			try
			{
				var json = JsonSerializer.Serialize(value);
				await _db.HashSetAsync(hashKey, id, json);
				if (expiry.HasValue)
				{
					await _db.KeyExpireAsync(hashKey, expiry.Value);
				}
				return true;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}
			
		}

		public async Task<bool> StringDeleteAsync(string key)
		{
			try
			{
				await _db.KeyDeleteAsync(key);
				return true;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}
		}

		public async Task<bool> StringDeleteAsync(IEnumerable<string> keys)
		{
			try
			{
				var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
				await _db.KeyDeleteAsync(redisKeys); 
				return true;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}
		}

		public async Task<bool> HashDeleteAsync(string hashKey, IEnumerable<int> ids)
		{
			try
			{
				RedisValue[] fields = ids.Select(id => (RedisValue)id).ToArray();
				 await _db.HashDeleteAsync(hashKey, fields);
				return true;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}
		}

		public async Task<List<int>> HashGetAsync(string hashKey)
		{
			List<int> result = new List<int>();
			try
			{
				HashEntry[] entries = await _db.HashGetAllAsync(hashKey);
				result = entries.Select(entry => (int)entry.Name).ToList();
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
			}

			return result;

		}
		
	}
}
