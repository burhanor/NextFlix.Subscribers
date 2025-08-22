using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Receiveds;
using NextFlix.Subscribers.Services;

namespace NextFlix.Subscribers
{
	public partial class Form1 : Form
	{
		private RabbitMqConnectionManager _connectionManager;
		private RabbitMqService _rabbitMqService;
		private CountryReceived _countryReceived;
		private TagReceived _tagReceived;
		private CategoryReceived _categoryReceived;
		private ChannelReceived _channelReceived;
		private CastReceived _castReceived;
		private SourceReceived _sourceReceived;
		private UserReceived _userReceived;
		private IRedisService _redisService;



		public Form1(IRedisService redisService, CountryReceived countryReceived, TagReceived tagReceived, CategoryReceived categoryReceived, ChannelReceived channelReceived, CastReceived castReceived, SourceReceived sourceReceived, UserReceived userReceived)
		{

			_countryReceived = countryReceived;
			_tagReceived = tagReceived;
			_categoryReceived = categoryReceived;
			_channelReceived = channelReceived;
			_castReceived = castReceived;
			_sourceReceived = sourceReceived;
			_userReceived = userReceived;
			_redisService = redisService;
			_redisService.OnConnectionStatusChanged += RedisService_OnConnectionStatusChanged;
			InitializeComponent();
		}
		private void RedisService_OnConnectionStatusChanged(object sender, bool isConnected)
		{
			lblRedisStatus.Invoke((MethodInvoker)(() =>
			{
				lblRedisStatus.Text = isConnected ? "Redis Connected" : "Redis Disconnected";
				lblRedisStatus.ForeColor = isConnected ? Color.Green : Color.Red;
			}));
		}

		private async Task Connect()
		{
			_connectionManager = new RabbitMqConnectionManager();
			var connection = await _connectionManager.GetConnection();
			_rabbitMqService = new RabbitMqService(connection);
			await Task.WhenAll(
				_rabbitMqService.Subscribe("Countries", _countryReceived.Received),
				_rabbitMqService.Subscribe("Tags", _tagReceived.Received),
				_rabbitMqService.Subscribe("Categories", _categoryReceived.Received),
				_rabbitMqService.Subscribe("Channels", _channelReceived.Received),
				_rabbitMqService.Subscribe("Casts", _castReceived.Received),
				_rabbitMqService.Subscribe("Sources", _sourceReceived.Received),
				_rabbitMqService.Subscribe("Users", _userReceived.Received)
				);

		}


		private void Form1_Load(object sender, EventArgs e)
		{

			RedisService_OnConnectionStatusChanged(null, _redisService.IsConnected);
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			Task.Run(async () =>
			{
				await Connect();
			});
			btnStart.Enabled = false;
			btnStop.Enabled = true;
		}

		private void btnStop_Click(object sender, EventArgs e)
		{

			btnStop.Enabled = false;
			btnStart.Enabled = true;
		}
	}
}
