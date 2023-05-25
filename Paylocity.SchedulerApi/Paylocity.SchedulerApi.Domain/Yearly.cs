namespace Paylocity.SchedulerApi.Domain
{
  public class Yearly : IFrequency
  {
    public string Name => "Yearly";

    public TimeSpan TimeOfDay { get; private set; }

    public DateTime Start { get; private set; }

    public DateTime? End { get; private set; }

    public IReadOnlyList<DateTime> Days { get; private set; } = new List<DateTime>();

    public bool IsRecurring => true;

    public int? NumberOfRecurrences { get; private set; }
    public int Every { get; private set; } = 0;
    public int Skip { get; private set; } = 0;

    public IEnumerable<DateTime> TriggerDates => throw new NotImplementedException();

    public DateTime? GetFirstEventDate()
    {
      throw new NotImplementedException();
    }

    public DateTime? GetNextEventDate(DateTime? lastTriggered)
    {
      throw new NotImplementedException();
    }
  }
}
