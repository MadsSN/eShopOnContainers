using Fund.API.Infrastructure.Exceptions;
using System;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Model
{
    public class Account
    {
        public int Id { get; set; }

        public decimal Credit { get; set; }

        public Account() { }
    }
}
