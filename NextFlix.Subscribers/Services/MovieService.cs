using Dapper;
using Microsoft.Data.SqlClient;
using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Models.Movie;
using System.Data;

namespace NextFlix.Subscribers.Services
{
	public class MovieService
	{
		private readonly string _connectionString = AppConfiguration.GetValue("ConnectionStrings:MSSQLConnection");

		public MovieService()
		{
		}
		public async Task<MovieModel?> GetMovie(int movieId)
		{
			using var connection = new SqlConnection(_connectionString);

			using var multi = await connection.QueryMultipleAsync(
			"GetMovieDetails",
			new { MovieId = movieId },
			commandType: CommandType.StoredProcedure
			);

			var movie = await multi.ReadFirstOrDefaultAsync<MovieModel>();
			if (movie is null)
				return null;

			movie.User = await multi.ReadFirstOrDefaultAsync<UserResponse>() ?? new();
			movie.Tags = (await multi.ReadAsync<MovieTagResponse>()).ToList();
			movie.Categories = (await multi.ReadAsync<MovieCategoryResponse>()).ToList();
			movie.Channels = (await multi.ReadAsync<MovieChannelResponse>()).ToList();
			movie.Countries = (await multi.ReadAsync<MovieCountryResponse>()).ToList();
			movie.Casts = (await multi.ReadAsync<MovieCastResponse>()).ToList();
			movie.Trailers = (await multi.ReadAsync<MovieTrailerDto>()).ToList();
			movie.Sources = (await multi.ReadAsync<MovieSourceResponse>()).ToList();
			movie.ViewCount = await multi.ReadFirstOrDefaultAsync<int>();
			movie.Votes = (await multi.ReadAsync<MovieVoteResponse>()).ToList();

			return movie;
		}

		public async Task<List<int>> GetMovieIdFromCastId(int castId) 
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT MovieId FROM MovieCasts WHERE CastId = @CastId";
			var movieIds = await connection.QueryAsync<int>(query, new { CastId = castId });
			return movieIds.ToList();
		}

		public async Task<List<int>> GetMovieIdFromCategoryId(int categoryId)
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT MovieId FROM MovieCategories WHERE CategoryId = @CategoryId";
			var movieIds = await connection.QueryAsync<int>(query, new { CategoryId = categoryId });
			return movieIds.ToList();
		}

		public async Task<List<int>> GetMovieIdFromTagId(int tagId)
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT MovieId FROM MovieTags WHERE TagId = @TagId";
			var movieIds = await connection.QueryAsync<int>(query, new { TagId = tagId });
			return movieIds.ToList();
		}

		public async Task<List<int>> GetMovieIdFromChannelId(int channelId)
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT MovieId FROM MovieChannels WHERE ChannelId = @ChannelId";
			var movieIds = await connection.QueryAsync<int>(query, new { ChannelId = channelId });
			return movieIds.ToList();
		}

		public async Task<List<int>> GetMovieIdFromCountryId(int countryId)
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT MovieId FROM MovieCountries WHERE CountryId = @CountryId";
			var movieIds = await connection.QueryAsync<int>(query, new { CountryId = countryId });
			return movieIds.ToList();
		}

		public async Task<List<int>> GetMovieIdFromUserId(int userId)
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT Id FROM Movies WHERE UserId = @UserId";
			var movieIds = await connection.QueryAsync<int>(query, new { UserId = userId });
			return movieIds.ToList();
		}
		public async Task<List<int>> GetMovieIdFromSourceId(int sourceId)
		{
			using var connection = new SqlConnection(_connectionString);
			var query = "SELECT MovieId FROM MovieSources WHERE SourceId = @SourceId";
			var movieIds = await connection.QueryAsync<int>(query, new { SourceId = sourceId });
			return movieIds.ToList();
		}
	}
}
