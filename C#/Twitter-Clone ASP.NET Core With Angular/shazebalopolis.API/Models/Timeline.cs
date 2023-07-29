using System.ComponentModel.DataAnnotations;

namespace shazebs.api.Models
{
    public class Timeline
    {
        [Key]
        public long TimelineId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public List<Tweet> Tweets { get; set; }
    }
}
