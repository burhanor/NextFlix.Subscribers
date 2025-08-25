using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Receiveds;
using NextFlix.Subscribers.Services;

namespace NextFlix.Subscribers
{
	public partial class Form1 : Form
	{
		private RabbitMqConnectionManager _connectionManager;
		private CountryReceived _countryReceived;
		private TagReceived _tagReceived;
		private CategoryReceived _categoryReceived;
		private ChannelReceived _channelReceived;
		private CastReceived _castReceived;
		private SourceReceived _sourceReceived;
		private UserReceived _userReceived;
		private IRedisService _redisService;
		private MovieReceived _movieReceived;
		private MovieLikeReceived _movieLikesReceived;
		private MovieViewReceived _movieViewsReceived;
		private HangfireService _hangfireService;
		private IRabbitMqService _rabbitMqService;



		public Form1(
			IRedisService redisService,
			IRabbitMqService rabbitMqService,
			CountryReceived countryReceived,
			TagReceived tagReceived,
			CategoryReceived categoryReceived,
			ChannelReceived channelReceived,
			CastReceived castReceived,
			SourceReceived sourceReceived,
			UserReceived userReceived,
			MovieReceived movieReceived,
			MovieLikeReceived movieLikesReceived,
			MovieViewReceived movieViewsReceived,
			HangfireService hangfireService
			)
		{

			_countryReceived = countryReceived;
			_tagReceived = tagReceived;
			_categoryReceived = categoryReceived;
			_channelReceived = channelReceived;
			_castReceived = castReceived;
			_sourceReceived = sourceReceived;
			_userReceived = userReceived;
			_redisService = redisService;
			_movieReceived = movieReceived;
			_rabbitMqService = rabbitMqService;
			_movieLikesReceived = movieLikesReceived;
			_movieViewsReceived = movieViewsReceived;
			_redisService.OnConnectionStatusChanged += RedisService_OnConnectionStatusChanged;
			InitializeComponent();
			_hangfireService = hangfireService;
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
			await Task.WhenAll(
				_rabbitMqService.Subscribe("Countries", _countryReceived.Received),
				_rabbitMqService.Subscribe("Tags", _tagReceived.Received),
				_rabbitMqService.Subscribe("Categories", _categoryReceived.Received),
				_rabbitMqService.Subscribe("Channels", _channelReceived.Received),
				_rabbitMqService.Subscribe("Casts", _castReceived.Received),
				_rabbitMqService.Subscribe("Sources", _sourceReceived.Received),
				_rabbitMqService.Subscribe("Users", _userReceived.Received),
				_rabbitMqService.Subscribe("Movies", _movieReceived.Received),
				_rabbitMqService.Subscribe("MovieLikes", _movieLikesReceived.Received),
				_rabbitMqService.Subscribe("MovieViews", _movieViewsReceived.Received)
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
		}
	}
}
