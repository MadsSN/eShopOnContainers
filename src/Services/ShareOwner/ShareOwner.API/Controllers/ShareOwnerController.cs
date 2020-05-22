using ShareOwner.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShareOwnerController : ControllerBase
    {
        private readonly ShareOwnerContext _shareownerContext;
        private readonly ShareOwnerSettings _settings;
        private readonly IShareOwnerIntegrationEventService _shareownerIntegrationEventService;

        public ShareOwnerController(ShareOwnerContext context, IOptionsSnapshot<ShareOwnerSettings> settings, IShareOwnerIntegrationEventService catalogIntegrationEventService)
        {
            _shareownerContext = context ?? throw new ArgumentNullException(nameof(context));
            _shareownerIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/accounts
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<Model.ShareOwner>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ShareOwnersAsync(int stockId = 0, int stockTraderId = 0)
        {
            return Ok(await _shareownerContext.ShareOwners
                .Include(owner => owner.Reservations)
                .Where(owner => stockId == 0 || owner.StockId == stockId)
                .Where(owner => stockTraderId == 0 || owner.StockTraderId == stockTraderId)
                .ToListAsync());
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Model.ShareOwner), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Model.ShareOwner>> ShareOwnerByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _shareownerContext.ShareOwners.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item != null)
            {
                return item;
            }

            return NotFound();
        }
        
        [HttpPost]
        [Route("reserve")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Model.ShareOwner), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Model.ShareOwner>> UpdateAccountAsync([FromBody] Reservation reservation)
        {
            var shareOwner = await _shareownerContext.ShareOwners.SingleOrDefaultAsync(ci => ci.Id == reservation.ShareOwnerId);

            if (shareOwner == null)
            {
                return NotFound();
            }

            var reserve = new Reservation()
            {
                Reserved = reservation.Reserved,
                ShareOwnerId = shareOwner.Id
            };

            await _shareownerContext.Reservations.AddAsync(reserve);

            //Publish some event to notify that "Real" corrency have changed.. 
            //var @event = new AccountCreditChangedIntegrationEvent(item.StockTraderId, item.Credit - deposit, item.Credit);
           // await _shareownerIntegrationEventService.SaveEventAndShareOwnerContextChangesAsync(@event);
            //await _shareownerIntegrationEventService.PublishThroughEventBusAsync(@event);
            await _shareownerContext.SaveChangesAsync();

            return Ok(reserve);
        }
        

        /*
        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ShareOwner>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ShareOwner>>> ItemsWithNameAsync(string name, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var totalItems = await _shareownerContext.ShareOwners
                .Where(c => c.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _shareownerContext.ShareOwners
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<ShareOwner>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/1/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ShareOwner>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ShareOwner>>> ItemsByTypeIdAndBrandIdAsync(int catalogTypeId, int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<ShareOwner>)_shareownerContext.ShareOwners;

            root = root.Where(ci => ci.ShareOwnerTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.ShareOwnerBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<ShareOwner>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/all/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/all/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ShareOwner>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ShareOwner>>> ItemsByBrandIdAsync(int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<ShareOwner>)_shareownerContext.ShareOwners;

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.ShareOwnerBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<ShareOwner>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/ShareOwnerTypes
        [HttpGet]
        [Route("catalogtypes")]
        [ProducesResponseType(typeof(List<ShareOwnerType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ShareOwnerType>>> ShareOwnerTypesAsync()
        {
            return await _shareownerContext.ShareOwnerTypes.ToListAsync();
        }

        // GET api/v1/[controller]/ShareOwnerBrands
        [HttpGet]
        [Route("catalogbrands")]
        [ProducesResponseType(typeof(List<ShareOwnerBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ShareOwnerBrand>>> ShareOwnerBrandsAsync()
        {
            return await _shareownerContext.ShareOwnerBrands.ToListAsync();
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody]ShareOwner productToUpdate)
        {
            var catalogItem = await _shareownerContext.ShareOwners.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            catalogItem = productToUpdate;
            _shareownerContext.ShareOwners.Update(catalogItem);

            if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);

                // Achieving atomicity between original ShareOwner database operation and the IntegrationEventLog thanks to a local transaction
                await _shareownerIntegrationEventService.SaveEventAndShareOwnerContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await _shareownerIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                await _shareownerContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody]ShareOwner product)
        {
            var item = new ShareOwner
            {
                ShareOwnerBrandId = product.ShareOwnerBrandId,
                ShareOwnerTypeId = product.ShareOwnerTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _shareownerContext.ShareOwners.Add(item);

            await _shareownerContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = _shareownerContext.ShareOwners.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _shareownerContext.ShareOwners.Remove(product);

            await _shareownerContext.SaveChangesAsync();

            return NoContent();
        }

        private List<ShareOwner> ChangeUriPlaceholder(List<ShareOwner> items)
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
