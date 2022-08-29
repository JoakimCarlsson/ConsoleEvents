var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(x =>
{
    x.AddTransient<IEventService, EventService>();
    x.AddHostedService<WorkerService>();
});

var app = builder.Build();


app.Run();