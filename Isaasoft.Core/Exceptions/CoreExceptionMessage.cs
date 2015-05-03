namespace Isaasoft.Core.Exceptions
{
    public class CoreExceptionMessage
    {
        protected string key = string.Empty;

        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                if (value == null)
                {
                    this.key = string.Empty;
                }
                else
                {
                    this.key = value;
                }
            }
        }

        public string Message { get; set; }
    }
}
