using System.ComponentModel.DataAnnotations;

namespace shazebs.api.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }

        [Required]
        public string Username { get; set; }

        //[Required]
        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        //[Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<Tweet> Tweets { get; set; }

        public List<User> Friends { get; set; }

        public List<string> Badges { get; set; }
    }
}