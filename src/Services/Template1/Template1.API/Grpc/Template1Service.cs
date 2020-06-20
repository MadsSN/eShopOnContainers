using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Template1.API;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Template1.API.Infrastructure;
using Template1Api;
using static Template1Api.Template1;

namespace Template1.API.Grpc
{
    public class Template1Service : Template1Base
    {
        private readonly Template1Context _template1Context;
        private readonly Template1Settings _settings;
        private readonly ILogger _logger;
        public Template1Service(Template1Context dbContext, IOptions<Template1Settings> settings, ILogger<Template1Service> logger)
        {
            _settings = settings.Value;
            _template1Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        

       public override async Task<Template1ItemResponse> GetItemById(Template1ItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call Template1Service.GetItemById for product id {Id}", request.Id);
            if (!Guid.TryParse(request.Id, out Guid guid) || guid == Guid.Empty)
            {
                context.Status = new Status(global::Grpc.Core.StatusCode.FailedPrecondition, $"Id must be valid guid (received {request.Id})");
                return null;
            }

            var item = await _template1Context.Template1s.SingleOrDefaultAsync(ci => ci.Id == guid);

            if (item == null)
            {
                context.Status = new Status(global::Grpc.Core.StatusCode.NotFound, $"Product with id {request.Id} do not exist");
                return null;
            }

            return new Template1ItemResponse()
            {
                Id = item.Id.ToString(),
                Name = item.Name,
            };
        }
    }
}
