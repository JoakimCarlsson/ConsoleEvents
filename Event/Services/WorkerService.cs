using System.Collections.Specialized;

namespace Event.Services;

public class WorkerService : IHostedService
{
    private readonly IEventService _eventService;
    private int _counter;
    public WorkerService(IEventService eventService)
    {
        _eventService = eventService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // EventArgs(cancellationToken);
        SomethingElse(cancellationToken);
        return Task.CompletedTask;
    }

    private void SomethingElse(CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            _eventService.ObservableCollection.CollectionChanged += CollectionChanged;
            while (cancellationToken.IsCancellationRequested is false)
            {
                _counter++;

                if (_counter % 10000 == 0)
                {
                    _eventService.ObservableCollection.Add("Hello");
                }

                if (_counter % 50000 == 0)
                {
                    _eventService.ObservableCollection.Clear();
                }
            }
        }, cancellationToken);
    }

    private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems)
                {
                    Console.WriteLine($"{item} added");
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems)
                {
                    Console.WriteLine($"{item} removed");
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                foreach (var item in e.OldItems)
                {
                    Console.WriteLine($"{item} replaced");
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                Console.WriteLine("Reset");
                break;
        }
    }

    private void EventArgs(CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            _eventService.OnCounterChanged += (sender, e) => { Console.WriteLine($"Counter changed to {e.Value}"); };

            while (cancellationToken.IsCancellationRequested is false)
            {
                _eventService.IncrementCounter();
            }
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}