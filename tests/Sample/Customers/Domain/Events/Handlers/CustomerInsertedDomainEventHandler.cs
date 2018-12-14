﻿namespace Naos.Sample.Customers.Domain
{
    using System.Threading;
    using System.Threading.Tasks;
    using EnsureThat;
    using Microsoft.Extensions.Logging;
    using Naos.Core.Common;
    using Naos.Core.Domain;

    public class CustomerInsertedDomainEventHandler
        : IDomainEventHandler<EntityInsertedDomainEvent>
    {
        private readonly ILogger<CustomerInsertedDomainEventHandler> logger;

        public CustomerInsertedDomainEventHandler(ILogger<CustomerInsertedDomainEventHandler> logger)
        {
            EnsureArg.IsNotNull(logger, nameof(logger));

            this.logger = logger;
        }

        public bool CanHandle(EntityInsertedDomainEvent notification)
        {
            return notification?.Entity.Is<Customer>() == true;
        }

        public async Task Handle(EntityInsertedDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                if (this.CanHandle(notification))
                {
                    this.logger.LogInformation($"inserted entity: {notification.Entity.GetType().PrettyName()} handled by {this.GetType().PrettyName()}");

                    // TODO: do something, trigger message (integration)
                }
            });
        }
    }
}
