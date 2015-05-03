using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isaasoft.Core.Net
{
    public struct UrlParameter
    {
        public static bool operator ==(UrlParameter a, UrlParameter b)
        {
            return string.Equals(a.Name, b.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator ==(UrlParameter a, string name)
        {
            return string.Equals(a.Name, name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator !=(UrlParameter a, UrlParameter b)
        {
            return !string.Equals(a.Name, b.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator !=(UrlParameter a, string name)
        {
            return !string.Equals(a.Name, name, StringComparison.InvariantCultureIgnoreCase);
        }

        public UrlParameter(string name, string value)
            : this()
        {
            this.Name = name;
            this.Value = value;
        }

        public UrlParameter(string name, string[] values)
            : this()
        {
            this.Name = name;
            this.Values = values;
        }

        private string name;

        public string Name
        {
            get
            {
                if (name == null)
                    return "";

                return this.name;
            }
            set { this.name = value; }
        }

        public string Value
        {
            get
            {
                if (Values == null)
                    return "";

                return string.Join(",", Values);
            }
            set
            {
                Values = new[] { value };
            }
        }

        public IEnumerable<string> Values { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Value);
        }

        public override bool Equals(object obj)
        {
            return this == (UrlParameter)obj;
        }

        public override int GetHashCode()
        {
            return this.Name.ToLowerInvariant().GetHashCode();
        }

        public static UrlParameter Empty
        {
            get { return new UrlParameter(); }
        }
    }
}
