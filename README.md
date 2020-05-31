# AspNetCore.EventBus.RabbitMQ
An EventBus base on Asp.net core 3.1 and RabbitMq. 

Made by inspiration from [dotnet-architecture/eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers), however there are some changes:
- Replace Autofac with default asp.net core ioc container.
- Add some extention methods for adding event bus.
- Delayed to create rabbitmq channel for event publish.
- Class name changed.

## Add RabbitMq EventBus

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddEventBusRabbitMq(ops =>
    {
       ops.HostName = "localhost";
       ops.UserName = "guest";
       ops.Password = "guest";   
       ops.QueueName = "event_bus_queue";
       ops.BrokerName = "event_bus";
       ops.RetryCount = 5;
    });
}
```

## Add Custom EventBus

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddEventBus<CustomEventBus>();
}
```

**!important: You must Add any LoggerProvider**

## Publish

```csharp
public class UserLoginEvent:Event
{
   public string UserName { get; set; }
}

public class ValuesController : ControllerBase
{
  private readonly IEventBus _eventBus;

  public ValuesController(IEventBus eventBus)
  {
     _eventBus = eventBus;
  }

  [HttpGet]
  public ActionResult Get()
  {
     _eventBus.Publish(new UserLoginEvent() { UserName = "Peppa" });
     return Ok();
  }
}
```

## Subscribe

```csharp
public class UserLoginEventHandler : IEventHandler<UserLoginEvent>
{
   private readonly ILogger<UserLoginEventHandler> _logger;

   public UserLoginEventHandler(ILogger<UserLoginEventHandler> logger)
   {
       _logger = logger;
   }

   public Task Handle(UserLoginEvent @event)
   {
       _logger.LogInformation($"Welcome {@event.UserName}!");
       return Task.FromResult(0);
   }
}
```

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{        
    app.UseMvc();

    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<UserLoginEvent, UserLoginEventHandler>();
}
```






