using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Helpers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models;
using NextFlix.Subscribers.Models.Movie;
using NextFlix.Subscribers.Services;
using System.Text.Json;

namespace NextFlix.Subscribers.Receiveds
{
	public class MovieReceived(IRedisService redisService,IMeiliSearchService meiliSearchService, MovieService movieService) : GenericRedisReceived<IdModel>(redisService, RedisPrefix.Movie)
	{
		public override async Task<bool> Upsert(string message)
		{
			try
			{
				IdModel? model = JsonSerializer.Deserialize<IdModel>(message);
				if (model is null)
					return false;

				MovieModel? movie = await movieService.GetMovie(model.Id);
				if (movie is null)
					return false;
				bool result = await redisService.StringSetAsync($"{RedisPrefix.Movie}:{model.Id}", movie);
				if (result)
				{
					result = await redisService.HashSetAsync($"{RedisPrefix.Movie}", model.Id, movie);
				}

				if (result && movie.Status==Status.ACCEPTED)
				{
					result = await meiliSearchService.AddOrUpdateMovieAsync(movie);
				}
				return result;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}
		}
		public override async Task<bool> Delete(string message)
		{
		
			try
			{
				List<int>? ids = JsonSerializer.Deserialize<List<int>>(message);
				bool result = false;
				if (ids is null)
					return true;
				List<string> keys = ids.Select(id => $"{RedisPrefix.Movie}:{id}").ToList();
				result = await redisService.StringDeleteAsync(keys);
				if (result)
				{
					result = await redisService.HashDeleteAsync($"{RedisPrefix.Movie}", ids);
				}
				if (result)
				{
					result = await meiliSearchService.DeleteMoviesAsync(ids);
				}
				return result;
			}
			catch (Exception ex)
			{
				LogHelper.ExceptionLog(ex);
				return false;
			}
		}
	}
}
