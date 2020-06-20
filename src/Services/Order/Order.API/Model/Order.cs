using Order.API.Infrastructure.Exceptions;
using System;

namespace Microsoft.eShopOnContainers.Services.Order.API.Model
{

    public class Order
    {
        public int Id { get; set; }

        public int StockTraderId { get; set; }

        public int StockId { get; set; }

        public decimal SharesCount { get; set; }
        public OrderStatus Status { get; set; }

        public Order() { }
    }
    public class SalesOrder : Order
    {

        public SalesOrder() { }
    }

    public class BuyOrder : Order
    {
        public decimal PricePerShare { get; set; }
        public BuyOrder() { }
    }
}
