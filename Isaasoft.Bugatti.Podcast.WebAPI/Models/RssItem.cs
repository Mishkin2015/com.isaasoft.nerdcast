using Isaasoft.Core.Rss.Version2_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Isaasoft.Bugatti.Podcast.WebAPI.Models
{
    public class RssItem
    {
        static readonly Regex JOVEM_NERD_PODCAST_IMAGE_URL = new Regex( @"<img.*src=""(http:\/\/[^""]*)"".*/>", RegexOptions.Compiled | RegexOptions.IgnoreCase );

        public string HashKey { get; set; }

        public string Title { get; set; }

        public string HowLongAgo { get; set; }

        public DateTime PubDate { get; set; }

        public string ImageUrl { get; set; }

        public string Duration { get; set; }

        public Enclosure Enclosure { get; set; }

        public static RssItem Parse( string hashKey, Item item )
        {
            var model = new RssItem();

            model.HashKey = hashKey;
            model.Duration = item.ItDuration;
            model.ImageUrl = GetJovemNerdPodcastImageUrlFromContentEncoded( item.ContentEncoded );
            model.HowLongAgo = ParseHowLongAgo( item.PubDate );
            model.Title = item.Title;
            model.Enclosure = item.Enclosure;
            model.PubDate = item.PubDate;

            return model;
        }

        public static string ParseHowLongAgo( DateTime date )
        {
            if ( date > DateTime.Now ) throw new ArgumentOutOfRangeException( "date" );

            var diff = DateTime.Now - date;

            if ( diff.Days == 0 )
            {
                if ( diff.Hours == 0 )
                {
                    if ( diff.Minutes == 0 )
                    {
                        if ( diff.Seconds == 0 )
                        {
                            if ( diff.Milliseconds > 1 )
                                return diff.Milliseconds + " milisegundos atrás";

                            return diff.Milliseconds + " milisegundo atrás";
                        }

                        if ( diff.Seconds > 1 )
                            return diff.Seconds + " segundos atrás";

                        return diff.Seconds + " segundo atrás";
                    }

                    if ( diff.Minutes > 1 )
                        return diff.Minutes + " minutos atrás";

                    return diff.Minutes + " minuto atrás";
                }

                if ( diff.Hours > 1 )
                    return diff.Hours + " horas atrás";

                return diff.Hours + " hora atrás";
            }
            else if ( diff.Days < 7 )
            {
                if ( diff.Days > 1 )
                    return diff.Days + " dias atrás";

                return diff.Days + " dia atrás"; 
            }
            else if ( diff.Days >= 7 && diff.Days < 30)
            {
                var value = diff.Days / 7;

                if ( value > 1 )
                    return value + " semanas atrás";

                return value + " semana atrás";
            }
            else if ( diff.Days >= 30 && diff.Days < 365 )
            {
                var value = diff.Days / 30;

                if ( value > 1 )
                    return value + " meses atrás";

                return value + " mês atrás";
            }
           
            else if ( diff.Days >= 365 )
            {
                var value = diff.Days / 365;

                if ( value > 1 )
                    return value + " anos atrás";

                return value + " ano atrás";
            }

            throw new NotImplementedException();
        }

        public static string GetJovemNerdPodcastImageUrlFromContentEncoded( string contentEncoded )
        {
            return JOVEM_NERD_PODCAST_IMAGE_URL.Match( contentEncoded ).Groups[1].Value;
        }
    }
}