using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Fund.API;
using Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;
using Microsoft.eShopOnContainers.Services.Fund.API.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fund.API.Grpc
{
    public class FundService : ControllerBase
    {
        private readonly FundContext _catalogContext;
        private readonly FundSettings _settings;
        private readonly ILogger _logger;
        public FundService(FundContext dbContext, IOptions<FundSettings> settings, ILogger<FundService> logger)
        {
            _settings = settings.Value;
            _catalogContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        /*

        public override async Task<CatalogItemRequest> GetItemById(CatalogItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call FundService.GetItemById for product id {Id}", request.Id);
            if (request.Id <= 0)
            {
                context.Status = new Status(global::Grpc.Core.StatusCode.FailedPrecondition, $"Id must be > 0 (received {request.Id})");
                return null;
            }

            var item = await _catalogContext.Accounts.SingleOrDefaultAsync(ci => ci.Id == request.Id);
            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;
            item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);

            if (item != null)
            {
                return new FundItemResponse()
                {
                    AvailableStock = item.AvailableStock,
                    Description = item.Description,
                    Id = item.Id,
                    MaxStockThreshold = item.MaxStockThreshold,
                    Name = item.Name,
                    OnReorder = item.OnReorder,
                    PictureFileName = item.PictureFileName,
                    PictureUri = item.PictureUri,
                    Price = (double)item.Price,
                    RestockThreshold = item.RestockThreshold
                };
            }

            context.Status = new Status(global::Grpc.Core.StatusCode.NotFound, $"Product with id {request.Id} do not exist");
            return null;
        }

        public override async Task<PaginatedItemsResponse> GetItemsByIds(FundItemsRequest request, ServerCallContext context)
        {
            if (!string.IsNullOrEmpty(request.Ids))
            {
                var items = await GetItemsByIdsAsync(request.Ids);

                if (!items.Any())
                {
                    context.Status = new Status(global::Grpc.Core.StatusCode.NotFound, $"ids value invalid. Must be comma-separated list of numbers");
                }
                context.Status = new Status(global::Grpc.Core.StatusCode.OK, string.Empty);
                return this.MapToResponse(items);
            }

            var totalItems = await _catalogContext.Accounts
                .LongCountAsync();

            var itemsOnPage = await _catalogContext.Accounts
                .OrderBy(c => c.Name)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync();


            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = this.MapToResponse(itemsOnPage, totalItems, request.PageIndex, request.PageSize);
            context.Status = new Status(global::Grpc.Core.StatusCode.OK, string.Empty);

            return model;
        }

        private PaginatedItemsResponse MapToResponse(List<Account> items)
        {
            return this.MapToResponse(items, items.Count(), 1, items.Count());
        }

        private PaginatedItemsResponse MapToResponse(List<Account> items, long count, int pageIndex, int pageSize)
        {
            var result = new PaginatedItemsResponse()
            {
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            items.ForEach(i =>
            {
                var brand = i.FundBrand == null
                            ? null
                            : new FundBrand()
                            {
                                Id = i.FundBrand.Id,
                                Name = i.FundBrand.Brand,
                            };
                var catalogType = i.FundType == null
                                  ? null
                                  : new FundType()
                                  {
                                      Id = i.FundType.Id,
                                      Type = i.FundType.Type,
                                  };

                result.Data.Add(new FundItemResponse()
                {
                    AvailableStock = i.AvailableStock,
                    Description = i.Description,
                    Id = i.Id,
                    MaxStockThreshold = i.MaxStockThreshold,
                    Name = i.Name,
                    OnReorder = i.OnReorder,
                    PictureFileName = i.PictureFileName,
                    PictureUri = i.PictureUri,
                    RestockThreshold = i.RestockThreshold,
                    FundBrand = brand,
                    FundType = catalogType,
                    Price = (double)i.Price,
                });
            });

            return result;
        }


        private async Task<List<Account>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<Account>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _catalogContext.Accounts.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            items = ChangeUriPlaceholder(items);

            return items;
        }

        private List<Account> ChangeUriPlaceholder(List<Account> items)
        {
            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;

            foreach (var item in items)
            {
                item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
            }

            return items;
        }
        */
    }
}
