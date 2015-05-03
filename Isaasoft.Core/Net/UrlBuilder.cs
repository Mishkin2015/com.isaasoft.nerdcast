using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Isaasoft.Core.Net
{
    public class UrlBuilder
    {
        public UrlBuilder(string url)
        {
            this.Url = url;
            this.LeftPart = UrlBuilder.GetLeftPart(url);
            this.Query = UrlBuilder.GetQuery(url);
        }

        public string Url { get; private set; }

        public string LeftPart { get; private set; }

        public string Query { get; private set; }

        public Dictionary<string, UrlParameter> QueryParameters
        {
            get
            {
                return UrlBuilder.ConvertToParameter(this.Query ?? string.Empty);
            }
        }

        public UrlBuilder Add(string name, string value)
        {
            return this.Add(new UrlParameter(name, value));
        }

        public UrlBuilder Add(string name, string[] values)
        {
            return this.Add(new UrlParameter(name, values));
        }

        public UrlBuilder Add(params UrlParameter[] parameters)
        {
            var queryParameters = this.QueryParameters;

            foreach (var item in parameters)
                queryParameters.Add(item.Name, item);

            return new UrlBuilder(this.NewUrl(queryParameters.Select(i => i.Value).ToArray()));
        }

        public UrlBuilder AddOrUpdate(string name, string value)
        {
            return this.AddOrUpdate(new UrlParameter(name, value));
        }

        public UrlBuilder AddOrUpdate(string name, string[] values)
        {
            return this.AddOrUpdate(new UrlParameter(name, values));
        }

        public UrlBuilder AddOrUpdate(params UrlParameter[] parameters)
        {
            var queryParameters = this.QueryParameters;

            foreach (var item in parameters)
                queryParameters[item.Name] = item;

            return new UrlBuilder(this.NewUrl(queryParameters.Select(i => i.Value).ToArray()));
        }

        public UrlBuilder Remove(string name)
        {
            var queryParameters = this.QueryParameters;

            queryParameters.Remove(name);

            return new UrlBuilder(this.NewUrl(queryParameters.Select(i => i.Value).ToArray()));
        }

        public string NewUrl(params UrlParameter[] parameters)
        {
            var query = "";

            var build = new List<string>();

            foreach (var item in parameters.Where(item => item != UrlParameter.Empty))
            {
                foreach (var value in item.Values)
                    build.Add(string.Format("{0}={1}", item.Name, WebUtility.UrlEncode(value)));
            }

            if (!build.Any())
                return this.LeftPart;

            query = "?" + string.Join("&", build);

            return this.LeftPart + query;
        }

        public override string ToString()
        {
            return this.Url;
        }

        public static string GetLeftPart(string url)
        {
            var leftPart = Regex.Match(url, @".*\?").Value;

            if (leftPart == string.Empty)
                return url;

            if (leftPart.EndsWith("?"))
                return leftPart.Substring(0, leftPart.Length - 1);

            return leftPart;
        }

        public static string GetQuery(string url)
        {
            return Regex.Match(url, @"\?.*").Value;
        }

        public static Dictionary<string, UrlParameter> ConvertToParameter(string query)
        {
            var parameters = query.Split(new[] { '&', '?' })
                .Where(m => !string.IsNullOrEmpty(m))
                .Select(m =>
                {
                    var nameValue = m.Split(new[] { '=' });
                    var name = nameValue[0];
                    var value = nameValue.Count() > 1 ? nameValue[1] : "";

                    return new UrlParameter
                    {
                        Name = name,
                        Value = WebUtility.UrlDecode(value)
                    };
                }).ToList();

            var replicated = parameters
                .GroupBy(m => m)
                .Where(m => m.Count() > 1)
                .Select(m => m.ToList());

            foreach (var item in replicated)
            {
                parameters.RemoveAll(i => item.Contains(i));
                parameters.Add(new UrlParameter(item.First().Name, item.Select(m => m.Value).ToArray()));
            }

            return parameters.ToDictionary(i => i.Name, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
