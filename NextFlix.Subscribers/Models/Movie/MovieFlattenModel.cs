namespace NextFlix.Subscribers.Models.Movie
{
	public class MovieFlattenModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public int Duration { get; set; }
		public DateTime? PublishDate { get; set; }
		public string Slug { get; set; }
		public List<string>? Tags { get; set; }
		public List<string>? Categories { get; set; }
		public List<string>? Channels { get; set; }
		public List<string>? Countries { get; set; }
		public List<string>? Casts { get; set; }
		public List<int>? CategoryIds { get; set; }

		public List<int>? TagIds { get; set; }
		public List<int>? ChannelIds { get; set; }
		public List<int>? CountryIds { get; set; }
		public List<int>? CastIds { get; set; }

	}
}
