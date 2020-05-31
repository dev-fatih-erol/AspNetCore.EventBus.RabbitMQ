# AspNetCore.EventBus.RabbitMQ
An EventBus base on Asp.net core 3.1 and RabbitMQ. 

Made by inspiration from [dotnet-architecture/eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers), however there are some changes:
- Replace Autofac with default asp.net core ioc container.
- Add some extention methods for adding event bus.
- Delayed to create rabbitmq channel for event publish.
- Class name changed.

## Add RabbitMQ EventBus

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddEventBusRabbitMQ(o =>
    {
       o.HostName = "localhost";
       o.UserName = "guest";
       o.Password = "guest";   
       o.QueueName = "event_bus_queue";
       o.BrokerName = "event_bus";
       o.RetryCount = 5;
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
public class UserRegisterEvent : Event
{
   public string Name { get; set; }
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
     _eventBus.Publish(new UserRegisterEvent() { Name = "Fatih" });
     return Ok();
  }
}
```

## Subscribe

```csharp
public class UserRegisterEventHandler : IEventHandler<UserRegisterEvent>
{
   private readonly ILogger<UserRegisterEventHandler> _logger;

   public UserRegisterEventHandler(ILogger<UserRegisterEventHandler> logger)
   {
       _logger = logger;
   }

   public Task Handle(UserRegisterEvent @event)
   {
       _logger.LogInformation($"Welcome {@event.Name}!");
       return Task.FromResult(0);
   }
}
```

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{        
    app.UseMvc();

    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<UserRegisterEvent, UserRegisterEventHandler>();
}
```






