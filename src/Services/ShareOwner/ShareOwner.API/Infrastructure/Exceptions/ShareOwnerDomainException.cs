using System;

namespace ShareOwner.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class ShareOwnerDomainException : Exception
    {
        public ShareOwnerDomainException()
        { }

        public ShareOwnerDomainException(string message)
            : base(message)
        { }

        public ShareOwnerDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
