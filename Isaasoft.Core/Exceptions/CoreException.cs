using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Isaasoft.Core.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException()
            : this(null, (CoreExceptionMessage[])null)
        {
        }

        public CoreException(params CoreExceptionMessage[] messages)
            : this(null, messages.ToArray())
        {
        }

        public CoreException(params KeyValuePair<string, string>[] messages)
            : this(null, messages.ToArray())
        {
        }

        public CoreException(Exception innerException, params CoreExceptionMessage[] messages)
            : base("See the property \"ExceptionMessages\" for details", innerException)
        {
            this.ExceptionMessages = messages;
        }

        public CoreException(Exception innerException, params KeyValuePair<string, string>[] messages)
            : base("See the property \"ExceptionMessages\" for details", innerException)
        {
            this.ExceptionMessages = messages.Select(item => new CoreExceptionMessage
            {
                Key = item.Key,
                Message = item.Value
            });
        }

        public IEnumerable<CoreExceptionMessage> ExceptionMessages { get; protected set; }

        public override string Message
        {
            get
            {
                return string.Join(
                    Environment.NewLine,
                    this.ExceptionMessages
                        .Select(item => item.Key == string.Empty ?
                            item.Message :
                            string.Format("Key: {0}, Message: {1}", item.Key, item.Message))
                );
            }
        }
    }
}
