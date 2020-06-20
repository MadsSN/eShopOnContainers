namespace Microsoft.eShopOnContainers.Services.Order.API.Model
{
    public enum OrderStatus
    {
        PendingValidation, 
        PendingMatch,
        Cancelled,
        Failed,
        Matched
    }
}