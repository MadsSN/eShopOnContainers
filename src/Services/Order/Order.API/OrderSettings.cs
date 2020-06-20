namespace Microsoft.eShopOnContainers.Services.Order.API
{
    public class OrderSettings
    {
        public string PicBaseUrl { get;set;}

        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
	public bool AzureStorageEnabled { get; set; }
    }
}
