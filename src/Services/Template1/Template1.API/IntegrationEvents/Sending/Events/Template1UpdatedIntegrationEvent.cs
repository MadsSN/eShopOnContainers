using System;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Template1.API.IntegrationEvents.Sending.Events
{
    public class Template1UpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid Template1Id { get; private set; }

        public string Name { get; private set; }

        public Template1UpdatedIntegrationEvent(Guid id, string name)
        {
            Template1Id = id;
            Name = name;
        }
    }

    public class Template1CreatedIntegrationEvent : IntegrationEvent
    {
        public Guid Template1Id { get; private set; }

        public string Name { get; private set; }

        public Template1CreatedIntegrationEvent(Guid id, string name)
        {
            Template1Id = id;
            Name = name;
        }
    }

    public class Template1DeletedIntegrationEvent : IntegrationEvent
    {
        public Guid Template1Id { get; private set; }

        public Template1DeletedIntegrationEvent(Guid id)
        {
            Template1Id = id;
        }
    }
}
