using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;

namespace NextFlix.Subscribers.Models
{
	public class SourceModel:IId
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public Status Status { get; set; }
		public SourceType SourceType { get; set; }
	}
}
