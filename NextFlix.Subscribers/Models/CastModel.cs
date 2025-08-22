using NextFlix.Subscribers.Enums;
using NextFlix.Subscribers.Interfaces;

namespace NextFlix.Subscribers.Models
{
	public class CastModel:IId
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string? Avatar { get; set; }
		public Status Status { get; set; }
		public DateTime? BirthDate { get; set; }
		public string? Biography { get; set; }
		public int CountryId { get; set; }
		public CastType CastType { get; set; }
		public Gender Gender { get; set; }
	}
}
