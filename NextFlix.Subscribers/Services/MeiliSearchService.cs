using Meilisearch;
using NextFlix.Subscribers.ConfigurationManagers;
using NextFlix.Subscribers.Interfaces;
using NextFlix.Subscribers.Models.MeiliSearch;
using NextFlix.Subscribers.Models.Movie;
using System.Threading.Tasks;

namespace NextFlix.Subscribers.Services
{
	public class MeiliSearchService : IMeiliSearchService
	{
		private readonly MeilisearchClient _client;
		private readonly Meilisearch.Index _index;
		private readonly MeiliSearchModel meiliSearchModel = AppConfiguration.GetSection<MeiliSearchModel>("MeiliSearch");

		public MeiliSearchService()
		{
			_client = new MeilisearchClient(meiliSearchModel.Url, meiliSearchModel.MasterKey);
			_index = _client.Index(meiliSearchModel.IndexName);

			
			// Searchable ve filterable alanlar
			_index.UpdateSearchableAttributesAsync(new[] {
				 "title",
				 "description",
				 "casts",
				 "categories",
				 "tags",
				 "channels",
				 "countries"
			}).Wait();
			_index.UpdateFilterableAttributesAsync(new[] {
				  "categoryIds",
				  "tagIds",
				  "channelIds",
				  "countryIds",
				  "castIds",
				  "duration",
				  "publishDate",
			}).Wait();
		}

		public async Task<bool> AddOrUpdateMovieAsync(MovieModel movie)
		{
			var task = await _index.AddDocumentsAsync(new[] { ToFlattenModel(movie) });

			var info = await _client.WaitForTaskAsync(task.TaskUid);

			return info.Status == TaskInfoStatus.Succeeded;
		}

		public async Task<bool> AddOrUpdateMoviesAsync(IEnumerable<MovieModel> movies)
		{
			var task = await _index.AddDocumentsAsync(ToFlattenModel(movies));
			var info = await _client.WaitForTaskAsync(task.TaskUid);

			return info.Status == TaskInfoStatus.Succeeded;
		}
		public async Task<bool> DeleteMovieAsync(int movieId)
		{
			var task = await _index.DeleteDocumentsAsync([movieId]);
			var info = await _client.WaitForTaskAsync(task.TaskUid);
			return info.Status == TaskInfoStatus.Succeeded;
		}

		public async Task<bool> DeleteMoviesAsync(IEnumerable<int> movieIds)
		{
			var task = await _index.DeleteDocumentsAsync(movieIds);
			var info = await _client.WaitForTaskAsync(task.TaskUid);
			return info.Status == TaskInfoStatus.Succeeded;
		}

		private MovieFlattenModel ToFlattenModel(MovieModel movie)
		{
			return new MovieFlattenModel
			{
				Id = movie.Id,
				Title = movie.Title,
				Description = movie.Description,
				Duration = movie.Duration,
				Casts = movie.Casts?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Casts.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Name).ToList() : [],
				Categories = movie.Categories?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Categories.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Name).ToList() : [],
				Tags = movie.Tags?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Tags.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Name).ToList() : [],
				Channels = movie.Channels?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Channels.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Name).ToList() : [],
				Countries = movie.Countries?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Countries.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Name).ToList() : [],
				Slug = movie.Slug,
				PublishDate = movie.PublishDate,
				CategoryIds = movie.Categories?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Categories.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Id).ToList() : [],
				TagIds = movie.Tags?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Tags.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Id).ToList() : [],
				ChannelIds = movie.Channels?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Channels.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Id).ToList() : [],
				CountryIds = movie.Countries?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Countries.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Id).ToList() : [],
				CastIds = movie.Casts?.Count(m => m.Status == Enums.Status.ACCEPTED) > 0 ? movie.Casts.Where(m => m.Status == Enums.Status.ACCEPTED).Select(c => c.Id).ToList() : []


			};
		}
		private List<MovieFlattenModel> ToFlattenModel(IEnumerable<MovieModel> movies)
		{
			return movies.Select(ToFlattenModel).ToList();
		}
	}
}
