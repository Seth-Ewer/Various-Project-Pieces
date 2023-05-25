namespace Paylocity.SchedulerApi.Domain
{
  /// <summary>
  ///   This class represents a one time event.  It is used to calculate the
  ///   next event date for a schedule.
  /// </summary>
  public class Once : IFrequency
  {
    #region CONSTRUCTORS

    /// <summary>
    ///   This constructor is used to create a one time event that occurs on
    ///   the specified date and time.
    /// </summary>
    /// <param name="start"></param>
    public Once(DateTime start)
    {
      Start = start.Date;
      TimeOfDay = start.TimeOfDay;
    }

    #endregion CONSTRUCTORS

    public string Name => "Once";

    public TimeSpan TimeOfDay { get; private set; }

    public DateTime Start { get; private set; }

    public DateTime? End => null;

    public bool IsRecurring => false;

    public int? NumberOfRecurrences => 1;
    public int Every => 0;
    public int Skip => 0;

    public IEnumerable<DateTime> TriggerDates
    {
      get
      {
        var next = GetFirstEventDate();
        if (next == null)
        {
          return new List<DateTime>()
                     .AsEnumerable();
        }
        return new List<DateTime>
                   { next.Value }
                   .AsEnumerable();
      }
    }

    public DateTime? GetNextEventDate(DateTime? lastTriggered)
    {
      var next = Start.Date.Add(TimeOfDay);
      if (lastTriggered.HasValue && lastTriggered.Value > next)
      {
        return null;
      }
      return next;
    }

    public DateTime? GetFirstEventDate() => GetNextEventDate(null);
  }
}
