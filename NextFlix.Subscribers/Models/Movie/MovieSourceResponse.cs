using NextFlix.Subscribers.Enums;

namespace NextFlix.Subscribers.Models.Movie
{
	public class MovieSourceResponse
	{
		public string Link { get; set; }
		public byte DisplayOrder { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public Status Status { get; set; }
		public SourceType SourceType { get; set; }
	}
}