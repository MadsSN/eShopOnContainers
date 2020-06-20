using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Order.API;
using Microsoft.eShopOnContainers.Services.Order.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Order.API.Model;
using Microsoft.eShopOnContainers.Services.Order.API.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Order.API.Grpc
{
    public class OrderService : ControllerBase
    {
        private readonly OrderContext _catalogContext;
        private readonly OrderSettings _settings;
        private readonly ILogger _logger;
        public OrderService(OrderContext dbContext, IOptions<OrderSettings> settings, ILogger<OrderService> logger)
        {
            _settings = settings.Value;
            _catalogContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        /*

        public override async Task<CatalogItemRequest> GetItemById(CatalogItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call OrderService.GetItemById for product id {Id}", request.Id);
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
                return new OrderItemResponse()
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

        public override async Task<PaginatedItemsResponse> GetItemsByIds(OrderItemsRequest request, ServerCallContext context)
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

        private PaginatedItemsResponse MapToResponse(List<SalesOrder> items)
        {
            return this.MapToResponse(items, items.Count(), 1, items.Count());
        }

        private PaginatedItemsResponse MapToResponse(List<SalesOrder> items, long count, int pageIndex, int pageSize)
        {
            var result = new PaginatedItemsResponse()
            {
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            items.ForEach(i =>
            {
                var brand = i.OrderBrand == null
                            ? null
                            : new OrderBrand()
                            {
                                Id = i.OrderBrand.Id,
                                Name = i.OrderBrand.Brand,
                            };
                var catalogType = i.OrderType == null
                                  ? null
                                  : new OrderType()
                                  {
                                      Id = i.OrderType.Id,
                                      Type = i.OrderType.Type,
                                  };

                result.Data.Add(new OrderItemResponse()
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
                    OrderBrand = brand,
                    OrderType = catalogType,
                    Price = (double)i.Price,
                });
            });

            return result;
        }


        private async Task<List<SalesOrder>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<SalesOrder>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _catalogContext.Accounts.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            items = ChangeUriPlaceholder(items);

            return items;
        }

        private List<SalesOrder> ChangeUriPlaceholder(List<SalesOrder> items)
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
