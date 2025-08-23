using NextFlix.Subscribers.Enums;

namespace NextFlix.Subscribers.Models.Movie
{
	public class UserResponse
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Avatar { get; set; }
		public string Nickname { get; set; }
		public string EmailAddress { get; set; }
		public UserType UserType { get; set; }
		public string Slug { get; set; }
		public bool IsActive { get; set; }
	}
}