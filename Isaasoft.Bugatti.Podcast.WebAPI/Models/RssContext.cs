using Isaasoft.Core.Phonetic;
using Isaasoft.Core.Rss;
using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Models
{
    public static class RssContext
    {
        public static readonly Rss[] RSS = new[] 
        { 
            new Rss
            {
                RssId = 1,
                Name = "Jovem Nerd",
                RssUri = "http://jovemnerd.com.br/categoria/nerdcast/feed/"
            }
        };

        public static Dictionary<string, object> RssCache( int id )
        {
            var rssContent = RSS.First( i => i.RssId == id );

            if ( HttpContext.Current.Cache["RSS-" + id] == null )
            {
                var cacheValue = new Dictionary<string, object>();
                var callback = default( CacheItemRemovedCallback );

                cacheValue.Add( "RssContent", rssContent );
                cacheValue.Add( "CacheContext", HttpContext.Current.Cache );
                cacheValue.Add( "AppContext", HttpContext.Current.Application );

                callback = new CacheItemRemovedCallback( ( key, value, reason ) =>
                {
                    var valueUnbox = ( Dictionary<string, object> ) value;
                    var rssContentCache = ( Rss ) valueUnbox["RssContent"];
                    var cacheContext = ( Cache ) valueUnbox["CacheContext"];
                    var appContext = ( HttpApplicationState ) valueUnbox["AppContext"];

                    var rssChannel = RssConverter.ConvertTo<Rss<Channel>>( rssContentCache.RssUri );
                    var newCacheValue = new Dictionary<string, object>();
                    var minutes = int.Parse( ConfigurationManager.AppSettings["Cache:RssExpiration:Minutes"] );
                    var rssItemHash = new HashSet<RssItemHash>();

                    foreach ( var item in rssChannel.Channel.Items )
                    {
                        var titleComplexMetaphone = MetaphonePtBr.MetaphoneComplex( item.Title );
                        var summaryComplexMetaphone = MetaphonePtBr.MetaphoneComplex( item.ItSummary );

                        var md5 = MD5.Create();
                        byte[] inputBytes = Encoding.ASCII.GetBytes( item.Title + item.PubDateString + item.Guid.Value );
                        byte[] hashBytes = md5.ComputeHash( inputBytes );
                        var hashValue = string.Concat( hashBytes.Select( i => i.ToString( "X2" ) ).ToArray() );

                        rssItemHash.Add( new RssItemHash
                        {
                            HashKey = hashValue,
                            TheItem = item,
                            TitleComplexMetaphone = titleComplexMetaphone,
                            SummaryComplexMetaphone = summaryComplexMetaphone
                        } );
                    }

                    newCacheValue.Add( "RssContent", rssContentCache );
                    newCacheValue.Add( "CacheContext", cacheContext );
                    newCacheValue.Add( "AppContext", appContext );
                    newCacheValue.Add( "RssChannel", rssChannel );
                    newCacheValue.Add( "RssItemHash", rssItemHash.ToArray() );
                    newCacheValue.Add( "RefreshDate", DateTime.UtcNow );

                    cacheContext.Add( key, newCacheValue, null, DateTime.Now.AddMinutes( minutes ), TimeSpan.Zero, CacheItemPriority.High, callback );

                    appContext[key] = newCacheValue;
                } );

                callback( "RSS-" + id, cacheValue, CacheItemRemovedReason.Removed );
            }

            return ( Dictionary<string, object> ) HttpContext.Current.Cache["RSS-" + id];
        }

        public static Dictionary<string, object> RssApplication( int id )
        {
            if ( HttpContext.Current.Application["RSS-" + id] == null )
                return RssCache( id );

            return ( Dictionary<string, object> ) HttpContext.Current.Application["RSS-" + id];
        }
    }
}