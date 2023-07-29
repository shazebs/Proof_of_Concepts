using Microsoft.EntityFrameworkCore;
using shazebs.api.Models;

namespace shazebs.api
{
    public class DataContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }
        //public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opt"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Read all Tweet objects from the database.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Tweet> ReadAllTweetsFromDb()
        {
            return Tweets;
        }

        /// <summary>
        /// Read a Tweet object by its Id.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IQueryable<Tweet> ReadOneTweet(long key)
        {
            IQueryable<Tweet> entity = Tweets.Where(x => x.TweetId == key);
            return entity;
        }

        /// <summary>
        /// Insert a new Tweet object into the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Tweet> InsertTweetIntoDb(Tweet entity)
        {
            entity.TweetId = Tweets.Count() + 1;
            try
            {
                Tweets.Add(entity);
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            return entity;
        }

        /// <summary>
        /// Update a Tweet record in database by its TweetId.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTweetInDB(long key, Tweet entity)
        {
            try
            {
                var match = Tweets.FirstOrDefault(x => x.TweetId == key);
                if (match != null)
                {
                    match = entity;
                    await SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Delete a Tweet record in database by its TweetId.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTweetInDB(long key)
        {
            try
            {
                var match = Tweets.FirstOrDefault(x => x.TweetId == key);
                if (match != null)
                {
                    Tweets.Remove(match);
                    await SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
    }
}