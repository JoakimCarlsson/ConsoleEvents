namespace Event.Services;

public class EventService : IEventService
{
    public EventHandler<ValueChangedEventArgs>? OnCounterChanged { get; set; }

    public int Counter { get; private set; }

    public void IncrementCounter()
    {
        Counter++;
        
        //raise event for every 1k change 
        if (Counter % 1000 == 0)
        {
            OnCounterChanged?.Invoke(this, new ValueChangedEventArgs
            {
                Value = Counter
            });
        }
    }
}

public interface IEventService
{
    public int Counter { get; }

    public void IncrementCounter();
    public EventHandler<ValueChangedEventArgs>? OnCounterChanged { get; set; }
}

public sealed class ValueChangedEventArgs : EventArgs
{
    public int Value { get; init; }
}