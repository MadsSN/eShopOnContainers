namespace Microsoft.eShopOnContainers.Services.ShareOwner.API
{
    public class ShareOwnerSettings
    {
        public string PicBaseUrl { get;set;}

        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
	public bool AzureStorageEnabled { get; set; }
    }
}
