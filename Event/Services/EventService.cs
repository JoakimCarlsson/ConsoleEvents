using System.Collections.ObjectModel;

namespace Event.Services;

public class EventService : IEventService
{
    public EventHandler<ValueChangedEventArgs>? OnCounterChanged { get; set; }
    public ObservableCollection<string> ObservableCollection { get; } = new();
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
    
    public void AddToObservableCollection(string item)
    {
        ObservableCollection.Add(item);
    }
    
    public void ClearObservableCollection()
    {
        ObservableCollection.Clear();
    }
}

public interface IEventService
{
    public void IncrementCounter();
    public EventHandler<ValueChangedEventArgs>? OnCounterChanged { get; set; }
    public ObservableCollection<string> ObservableCollection { get; } 
}

public sealed class ValueChangedEventArgs : EventArgs
{
    public int Value { get; init; }
}