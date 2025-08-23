using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;

namespace NextFlix.Subscribers.Models.Movie
{
	public class MovieModel:IId
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public int Duration { get; set; }
		public Status Status { get; set; }
		public UserResponse User { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? PublishDate { get; set; }
		public string? Poster { get; set; }
		public string Slug { get; set; }
		public List<MovieTagResponse>? Tags { get; set; }
		public List<MovieCategoryResponse>? Categories { get; set; }
		public List<MovieChannelResponse>? Channels { get; set; }
		public List<MovieCountryResponse>? Countries { get; set; }
		public List<MovieCastResponse>? Casts { get; set; }
		public List<MovieTrailerDto>? Trailers { get; set; }
		public List<MovieSourceResponse>? Sources { get; set; }
		public int ViewCount { get; set; }
		public List<MovieVoteResponse>? Votes { get; set; }
	}
}
