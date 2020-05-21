using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Model
{
    public static class FundItemExtensions
    {
        public static void FillProductUrl(this FundItem item, string picBaseUrl, bool azureStorageEnabled)
        {
            if (item != null)
            {
                item.PictureUri = azureStorageEnabled
                   ? picBaseUrl + item.PictureFileName
                   : picBaseUrl.Replace("[0]", item.Id.ToString());
            }
        }
    }
}
