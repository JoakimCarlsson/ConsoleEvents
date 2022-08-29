namespace Event.Services;

public class WorkerService : IHostedService
{
    private readonly IEventService _eventService;

    public WorkerService(IEventService eventService)
    {
        _eventService = eventService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            _eventService.OnCounterChanged += (sender, e) =>
            {
                Console.WriteLine($"Counter changed to {e.Value}");
            };
            
            while (cancellationToken.IsCancellationRequested is false)
            {
                _eventService.IncrementCounter();
            }
        }, cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}