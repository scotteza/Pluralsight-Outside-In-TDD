using System.Net.Http;
using System.Web.Http;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        // This naming (starting with "Get") defines this as a GET method
        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse();
        }

        public HttpResponseMessage Post()
        {
            return this.Request.CreateResponse();
        }
    }
}
