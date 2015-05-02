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
    public class RssItemController : ApiController
    {
        // GET: /rss/{id}/item/
        public RssData Get( int id, string filter = null, int? start = null, int? limit = null )
        {
            var rssApp = RssContext.RssApplication( id );
            var rssItemHash = ( RssItemHash[] ) rssApp["RssItemHash"];
            var sourceItems = default( RssItemHash[] );
            var sourcePage = default( RssItemHash[] );

            if ( filter == null )
                sourceItems = rssItemHash.ToArray();
            else
            {
                var filterMetaphoneComplex = MetaphonePtBr.MetaphoneComplex( filter );
                var keywords = filterMetaphoneComplex.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
                var results = new HashSet<RssItemHash>();

                foreach ( var keyword in keywords )
                {
                    foreach ( var item in rssItemHash.Where( item => item.TitleComplexMetaphone.IndexOf( keyword, StringComparison.CurrentCultureIgnoreCase ) > -1 ) )
                        results.Add( item );

                    foreach ( var item in rssItemHash.Where( item => item.SummaryComplexMetaphone.IndexOf( keyword, StringComparison.CurrentCultureIgnoreCase ) > -1 ) )
                        results.Add( item );
                }

                sourceItems = results.ToArray();
            }

            sourcePage = sourceItems;

            if ( start != null )
                sourcePage = sourcePage.Skip( start.Value ).ToArray();

            if ( limit != null )
                sourcePage = sourcePage.Take( limit.Value ).ToArray();

            var pageLastIndex = ( start ?? 0 ) + sourcePage.Length;
            var authority = Request.RequestUri.GetLeftPart( UriPartial.Authority );

            return new RssData
            {
                Data = sourcePage.Select( item => RssItem.Parse( item.HashKey, item.TheItem ) ).ToArray(),
                FromIndex = start ?? 0,
                Amount = sourceItems.Length,
                Url = this.Request.RequestUri.ToString(),
                NextUrl = pageLastIndex < sourceItems.Length ? Url.Link( "RssItemApi", new { id = id, start = pageLastIndex, limit = limit, filter = filter } ) : null,
                PrevUrl = limit != null && start >= limit ? Url.Link( "RssItemApi", new { id = id, start = start - limit, limit = limit, filter = filter } ) : null
            };
        }
    }
}
