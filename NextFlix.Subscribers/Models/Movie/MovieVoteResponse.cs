using NextFlix.Subscribers.Enums;

namespace NextFlix.Subscribers.Models.Movie
{
	public class MovieVoteResponse
	{
		public VoteType Vote { get; set; }
		public int UniqueVoteCount { get; set; }
	}
}