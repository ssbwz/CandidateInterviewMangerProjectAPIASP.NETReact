using System.Runtime.Serialization;

namespace LogicLayer.Exceptions
{
    [Serializable]
    public class MSGraphException : Exception
    {
        private Exception ex;
        private string v;

        public MSGraphException()
        {
        }

        public MSGraphException(string? message) : base(message)
        {
        }

        public MSGraphException(Exception ex, string v) : base(v, ex)
        {
            this.ex = ex;
            this.v = v;
        }

        public MSGraphException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MSGraphException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}