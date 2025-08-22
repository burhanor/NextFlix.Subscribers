using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;

namespace NextFlix.Subscribers.Models
{
	public class ChannelModel:IId
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public Status Status { get; set; }
	}
}
