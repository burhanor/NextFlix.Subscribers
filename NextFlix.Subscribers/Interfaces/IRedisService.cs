namespace NextFlix.Subscribers.Interfaces
{
	public interface IRedisService
	{
		Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null);
		Task<bool> HashSetAsync<T>(string hashKey, int id, T value, TimeSpan? expiry = null);
		Task<bool> StringDeleteAsync(string key);
		Task<bool> StringDeleteAsync(IEnumerable<string> keys);
		Task<bool> HashDeleteAsync(string hashKey, IEnumerable<int> ids);
		public bool IsConnected { get; set; }
		public event EventHandler<bool> OnConnectionStatusChanged;
	}
}
