using NextFlix.Subscribers.Enums;

namespace NextFlix.Subscribers.Models.Movie
{
	public class MovieCategoryResponse
	{
		public byte DisplayOrder { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public Status Status { get; set; }
	}
}