using ShareOwner.API.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Amqp.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.Model
{
    public class ShareOwner
    {
        public int Id { get; set; }
        public int StockId { get; set; }

        public int StockTraderId { get; set; }

        public decimal Shares { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public ShareOwner() { }
    }

    public class Reservation
    {
        public int Id { get; set; }

        public int ShareOwnerId { get; set; }

        public decimal Reserved { get; set; }

        public Reservation()
        {
        }
    }
}
