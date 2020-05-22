namespace Microsoft.eShopOnContainers.Services.Stock.API
{
    public class StockSettings
    {
        public string PicBaseUrl { get;set;}

        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
	public bool AzureStorageEnabled { get; set; }
    }
}
