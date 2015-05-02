using Isaasoft.Bugatti.Podcast.WebAPI.Models;
using Isaasoft.Core.Rss;
using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Controllers
{
    public class RssController : ApiController
    {
        // GET: rss/
        public Rss[] Get()
        {
            return RssContext.RSS;
        }
    }
}
