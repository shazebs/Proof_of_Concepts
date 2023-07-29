using System.ComponentModel.DataAnnotations;

namespace shazebs.api.Models
{
    public class Friends
    {
        [Key]
        public long FriendsId { get; set; }

        [Required]
        public User Initiator { get; set; }

        [Required]
        public User Acceptor { get; set; }

        [Required]
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public Dictionary<string, string> Messages { get; set; } // message/date


    }
}
