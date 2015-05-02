using Isaasoft.Bugatti.Podcast.WebAPI.Models;
using Isaasoft.Core.Phonetic;
using Isaasoft.Core.Rss;
using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Controllers
{
    public class RssItemDetailController : ApiController
    {
        // GET: /rss/{id}/detail/
        public IHttpActionResult Get( int id, string hashKey )
        {
            var rssApp = RssContext.RssApplication( id );
            var rssItemHash = ( RssItemHash[] ) rssApp["RssItemHash"];
            var itemHash = rssItemHash.SingleOrDefault( item => item.HashKey == hashKey );

            if ( itemHash != null )
            {
                return Ok( new RssItemDetail
                {
                    HashKey = itemHash.HashKey,
                    ContentEncoded = itemHash.TheItem.ContentEncoded
                } );
            }

            return NotFound();
        }
    }
}
