using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Template1.API;
using Microsoft.Extensions.Options;
using Template1.API.Controllers.Request;
using Template1.API.Infrastructure;
using Template1.API.IntegrationEvents;
using Template1.API.IntegrationEvents.Sending.Events;


namespace Template1.API.Controllers
{
    using Model;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class Template1Controller : ControllerBase
    {
        private readonly Template1Context _template1Context;
        private readonly Template1Settings _settings;
        private readonly ITemplate1IntegrationEventService _template1IntegrationEventService;

        public Template1Controller(Template1Context context, IOptionsSnapshot<Template1Settings> settings, ITemplate1IntegrationEventService template1IntegrationEventService)
        {
            _template1Context = context ?? throw new ArgumentNullException(nameof(context));
            _template1IntegrationEventService = template1IntegrationEventService ?? throw new ArgumentNullException(nameof(template1IntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/accounts
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<Template1>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Template1sAsync()
        {
            return Ok(await _template1Context.Template1s.ToListAsync());
        }


        [HttpGet]
        [Route("/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Template1), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Template1>> Template1ByIdAsync(Guid id)
        {
            var item = await _template1Context.Template1s.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPut]
        [Route("/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Template1), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Template1>> UpdateTemplate1Async(Guid id, UpdateTemplate1Request request)
        {
            var item = await _template1Context.Template1s.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = request.Name;
            _template1Context.Template1s.Update(item);

            var @event = new Template1UpdatedIntegrationEvent(item.Id, item.Name);
            await _template1IntegrationEventService.SaveEventAndTemplate1ContextChangesAsync(@event);
            await _template1IntegrationEventService.PublishThroughEventBusAsync(@event);

            return Ok(item);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Template1), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Template1>> CreateTemplate1Async(CreateTemplate1Request request)
        {
            var item = new Template1()
            {
                Name = request.Name
            };

            _template1Context.Template1s.Add(item);

            var @event = new Template1CreatedIntegrationEvent(item.Id, item.Name);
            await _template1IntegrationEventService.SaveEventAndTemplate1ContextChangesAsync(@event);
            await _template1IntegrationEventService.PublishThroughEventBusAsync(@event);

            return CreatedAtAction(nameof(Template1ByIdAsync), new { id = item.Id }, item);
        }

        //Implement soft delete maybe?
        [HttpDelete]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Template1), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Template1>> DeleteTemplate1Async(Guid id)
        {
            var item = await _template1Context.Template1s.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _template1Context.Template1s.Remove(item);

            var @event = new Template1DeletedIntegrationEvent(item.Id);
            await _template1IntegrationEventService.SaveEventAndTemplate1ContextChangesAsync(@event);
            await _template1IntegrationEventService.PublishThroughEventBusAsync(@event);

            return NoContent();
        }
    }
}
