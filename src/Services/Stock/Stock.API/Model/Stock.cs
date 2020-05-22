using Stock.API.Infrastructure.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.eShopOnContainers.Services.Stock.API.Model
{
    public class Stock
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Range(0d, 1000000000d)]
        public decimal Price { get; set; }

        [Range(1, 000000000d)]
        public int TotalShares { get; set; }

        public Stock() { }
    }
}
