using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using shazebs.api.Models;

namespace shazebs.api.Controllers
{
    [Authorize]
    [EnableCors("default")]
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ODataController
    {
        private readonly DataContext _context;

        public TweetsController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/tweets
        /// </summary>
        /// <returns></returns>
        [ODataRoute("Tweets")]
        [EnableQuery]
        public IQueryable<Tweet> Get()
        {
            return _context.ReadAllTweetsFromDb();
        }

        /// <summary>
        /// GET: api/tweets/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ODataRoute("Tweets({key})")]
        [EnableQuery]
        public SingleResult<Tweet> Get([FromODataUri] long key)
        {
            return SingleResult.Create(_context.ReadOneTweet(key));
        }

        /// <summary>
        /// POST: api/tweets/{entity}
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ODataRoute("Tweets")]
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Tweet entity)
        {
            if (entity == null) return BadRequest(entity);

            var result = await _context.InsertTweetIntoDb(entity);

            if (result == null) return BadRequest(result);

            return Created(result);
        }

        /// <summary>
        /// PUT: api/tweets/{entity}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IActionResult> Update([FromODataUri] long key, [FromBody] Tweet entity)
        {
            throw new NotImplementedException("Tweets Update endpoint not implemented.");
        }

        /// <summary>
        /// DELETE: api/tweets/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IActionResult> Delete([FromODataUri] long key)
        {
            throw new NotImplementedException("Tweets Delete endpoint not implemented.");
        }
    }
}