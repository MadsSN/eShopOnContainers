using ShareOwner.API.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Azure.Amqp.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.Model
{
    public class ShareOwner : IValidatableObject
    {
        public int Id { get; set; }
        public int StockId { get; set; }

        public int StockTraderId { get; set; }

        public decimal Shares { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Reservations.Where(reservation => reservation.Status == ReservationStatus.Reserved).Sum(reservation => reservation.Reserved) > Shares)
            {
                yield return new ValidationResult(
                    "Cannot reserve more than total owned shares",
                    new[] { nameof(Reservations), nameof(Shares) });
            }
        }

        public ShareOwner() { }
    }

    public class Reservation : IValidatableObject
    {
        public int Id { get; set; }

        public int SalesOrderId { get; set; }

        public int ShareOwnerId { get; set; }

        public decimal Reserved { get; set; }

        public ReservationStatus Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Reserved <= 0)
            {
                yield return new ValidationResult(
                    "Must reserve a positive amount. ",
                    new[] { nameof(Reserved) });
            }
        }
        public Reservation()
        {
        }
    }

    public enum ReservationStatus
    {
        Completed,
        Cancelled,
        Reserved
    }
}
