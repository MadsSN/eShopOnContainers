using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class AccountCreditChangedIntegrationEvent : IntegrationEvent
    {
        public int StockTraderId { get; private set; }

        public decimal OldCredit { get; private set; }

        public decimal NewCredit { get; set; }

        public AccountCreditChangedIntegrationEvent(int stockTraderId, decimal oldCredit, decimal newCredit)
        {
            StockTraderId = stockTraderId;
            OldCredit = oldCredit;
            NewCredit = newCredit;
        }
    }
}