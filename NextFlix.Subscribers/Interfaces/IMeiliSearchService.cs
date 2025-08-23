using NextFlix.Subscribers.Models.Movie;

namespace NextFlix.Subscribers.Interfaces
{
	public interface IMeiliSearchService
	{
		Task<bool> AddOrUpdateMovieAsync(MovieModel movie);
		Task<bool> AddOrUpdateMoviesAsync(IEnumerable<MovieModel> movies);
		Task<bool> DeleteMovieAsync(int movieId);
		Task<bool> DeleteMoviesAsync(IEnumerable<int> movieIds);
	}
}
