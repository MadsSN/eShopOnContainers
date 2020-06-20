using System;

namespace Order.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class OrderDomainException : Exception
    {
        public OrderDomainException()
        { }

        public OrderDomainException(string message)
            : base(message)
        { }

        public OrderDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
