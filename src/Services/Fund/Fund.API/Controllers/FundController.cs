using Fund.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

namespace Microsoft.eShopOnContainers.Services.Fund.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly FundContext _fundContext;
        private readonly FundSettings _settings;
        private readonly IFundIntegrationEventService _fundIntegrationEventService;

        public FundController(FundContext context, IOptionsSnapshot<FundSettings> settings, IFundIntegrationEventService catalogIntegrationEventService)
        {
            _fundContext = context ?? throw new ArgumentNullException(nameof(context));
            _fundIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/accounts
        [HttpGet]
        [Route("accounts")]
        [ProducesResponseType(typeof(IEnumerable<Account>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AccountsAsync()
        {
            return Ok(await _fundContext.Accounts.ToListAsync());
        }


        [HttpGet]
        [Route("accounts/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Account), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Account>> AccountByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _fundContext.Accounts.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        [HttpPut]
        [Route("accounts/{stockTraderId:int}/{deposit:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Account), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Account>> UpdateAccountAsync(int stockTraderId, int deposit)
        {
            if (stockTraderId <= 0)
            {
                return BadRequest();
            }

            var item = await _fundContext.Accounts.SingleOrDefaultAsync(ci => ci.StockTraderId == stockTraderId);


            if (item == null)
            {
                return NotFound();
            }

            item.Credit += deposit;
            _fundContext.Accounts.Update(item);

            var @event = new AccountCreditChangedIntegrationEvent(item.StockTraderId, item.Credit - deposit, item.Credit);
            await _fundIntegrationEventService.SaveEventAndFundContextChangesAsync(@event);
            await _fundIntegrationEventService.PublishThroughEventBusAsync(@event);

            return Ok(item);
        }


        /*
        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Account>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Account>>> ItemsWithNameAsync(string name, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var totalItems = await _fundContext.Accounts
                .Where(c => c.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _fundContext.Accounts
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<Account>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/1/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Account>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Account>>> ItemsByTypeIdAndBrandIdAsync(int catalogTypeId, int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<Account>)_fundContext.Accounts;

            root = root.Where(ci => ci.FundTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.FundBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<Account>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/all/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/all/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Account>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Account>>> ItemsByBrandIdAsync(int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<Account>)_fundContext.Accounts;

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.FundBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<Account>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/FundTypes
        [HttpGet]
        [Route("catalogtypes")]
        [ProducesResponseType(typeof(List<FundType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<FundType>>> FundTypesAsync()
        {
            return await _fundContext.FundTypes.ToListAsync();
        }

        // GET api/v1/[controller]/FundBrands
        [HttpGet]
        [Route("catalogbrands")]
        [ProducesResponseType(typeof(List<FundBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<FundBrand>>> FundBrandsAsync()
        {
            return await _fundContext.FundBrands.ToListAsync();
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody]Account productToUpdate)
        {
            var catalogItem = await _fundContext.Accounts.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            catalogItem = productToUpdate;
            _fundContext.Accounts.Update(catalogItem);

            if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);

                // Achieving atomicity between original Fund database operation and the IntegrationEventLog thanks to a local transaction
                await _fundIntegrationEventService.SaveEventAndFundContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await _fundIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                await _fundContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody]Account product)
        {
            var item = new Account
            {
                FundBrandId = product.FundBrandId,
                FundTypeId = product.FundTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _fundContext.Accounts.Add(item);

            await _fundContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = _fundContext.Accounts.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _fundContext.Accounts.Remove(product);

            await _fundContext.SaveChangesAsync();

            return NoContent();
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
