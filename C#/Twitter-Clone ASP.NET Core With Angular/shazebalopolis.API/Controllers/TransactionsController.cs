using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shazebalopolis.API.Models;

namespace shazebalopolis.API.Controllers
{
    [EnableCors("default")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ODataController
    {
        public TransactionsController()
        {
        }

        [ODataRoute("Transactions")]
        [EnableQuery]
        public async Task<bool> Post([FromBody] Transaction transaction)
        {
            try
            {
                // use transaction object to process stripe payment.


                // uncomment line below once stripe integration is complete.
                // return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
    }
}