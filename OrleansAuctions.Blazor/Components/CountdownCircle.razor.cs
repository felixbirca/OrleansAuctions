using Microsoft.AspNetCore.Components;

namespace OrleansAuctions.Blazor.Components;

public partial class CountdownCircle : IDisposable
{
    private Timer? timer;
    private TimeSpan timeLeft;
    private string TimeDisplay => timeLeft.ToString(@"mm\:ss");
    private double totalDurationSeconds;

    [Parameter]
    public DateTimeOffset EndTime { get; set; }

    public string RemainingPathColor { get; set; } = "green"; // default to green
    public string RawTimeFraction { get; set; } = "283 283"; // default circle dasharray

    protected override void OnParametersSet()
    {
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        if (timer != null)
        {
            timer.Dispose(); // Dispose existing timer if reinitializing
        }
        timeLeft = EndTime - DateTimeOffset.Now;
        totalDurationSeconds = timeLeft.TotalSeconds;
        timer = new Timer(TimerCallback, null, 0, 1000); // Setup timer to start immediately
    }

    private void TimerCallback(object? state)
    {
        timeLeft = EndTime - DateTimeOffset.Now;

        if (timeLeft.TotalSeconds < 5)
        {
            RemainingPathColor = "red";
        }
        else if (timeLeft.TotalSeconds < 10)
        {
            RemainingPathColor = "orange";
        }

        double rawTimeFraction = timeLeft.TotalSeconds / totalDurationSeconds;
        RawTimeFraction = $"{(rawTimeFraction * 283).ToString("0")} 283";
        
        if (timeLeft <= TimeSpan.Zero)
        {
            timeLeft = TimeSpan.Zero; // Ensure timeLeft doesn't go negative
            timer?.Change(Timeout.Infinite, Timeout.Infinite);
            OnTimesUp();
        }

        InvokeAsync(StateHasChanged); // Re-render the component on the UI thread
    }

    private void OnTimesUp()
    {
        timer?.Dispose();
        Console.WriteLine("Time's up!");
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}