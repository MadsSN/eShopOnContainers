using System;

namespace Stock.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class StockDomainException : Exception
    {
        public StockDomainException()
        { }

        public StockDomainException(string message)
            : base(message)
        { }

        public StockDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
