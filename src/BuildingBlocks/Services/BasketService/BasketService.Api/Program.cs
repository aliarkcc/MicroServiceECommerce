using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.IntegrationEvents.EventHandlers;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServicesExt(builder.Services);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.RegisterWithConsul(app.Lifetime);

ConfigureSubscription(app.Services);

app.Run();

void ConfigureServicesExt(IServiceCollection services)
{
    services.ConfigureAuth(builder.Configuration);
    services.AddSingleton(sp => sp.ConfigureRedis(builder.Configuration));

    builder.Services.ConfigureConsul(builder.Configuration);

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IBasketRepository, RedisBasketRepository>();

    builder.Services.AddTransient<IIdentityService, IdentityService>();

    builder.Services.AddSingleton<IEventBus>(sp =>
    {
        EventBusConfig config = new EventBusConfig()
        {
            //Connection= "amqp://guest:guest@localhost:15672",
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "BasketService",
            EventBusType = EventBusType.RabbitMq
        };

        return EventBusFactory.Create(config, sp);
    });

    services.AddTransient<OrderCreatedIntegrationEventHandler>();
}

void ConfigureSubscription(IServiceProvider serviceProvider)
{
    IEventBus eventBus = serviceProvider.GetRequiredService<IEventBus>();
    eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
}
