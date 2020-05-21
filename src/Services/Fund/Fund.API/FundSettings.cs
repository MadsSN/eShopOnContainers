namespace Microsoft.eShopOnContainers.Services.Fund.API
{
    public class FundSettings
    {
        public string PicBaseUrl { get;set;}

        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
	public bool AzureStorageEnabled { get; set; }
    }
}
