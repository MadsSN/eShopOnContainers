using System;

namespace Fund.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class FundDomainException : Exception
    {
        public FundDomainException()
        { }

        public FundDomainException(string message)
            : base(message)
        { }

        public FundDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
