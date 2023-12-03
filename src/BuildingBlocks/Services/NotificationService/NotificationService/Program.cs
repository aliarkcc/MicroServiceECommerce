using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.EventHandlers;
using NotificationService.IntegrationEvents.Events;

ServiceCollection services = new ServiceCollection();

ConfigureServices(services);

var sp = services.BuildServiceProvider();

IEventBus eventBus = sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

Console.WriteLine("Notification Service is running");

Console.ReadLine();

static void ConfigureServices(ServiceCollection services)
{
    services.AddLogging(configure => configure.AddConsole());

    services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();
    services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();

    services.AddSingleton<IEventBus>(sp =>
    {
        EventBusConfig config = new EventBusConfig()
        {
            //Connection= "amqp://guest:guest@localhost:15672",
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "NotificationService",
            EventBusType = EventBusType.RabbitMq
        };

        return EventBusFactory.Create(config, sp);
    });
}