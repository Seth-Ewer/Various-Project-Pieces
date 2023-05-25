namespace Paylocity.SchedulerApi.Domain
{
  /// <summary>
  ///   This class represents a weekly event.  It is used to calculate the
  ///   next event date for a schedule.
  /// </summary>
  public class Weekly : IFrequency
  {
    #region CONSTRUCTORS

    /// <summary>
    ///  This constructor is used to create a weekly event that occurs on
    ///  the same day of the week as the start date.
    /// </summary>
    /// <param name="start"></param>
    public Weekly(DateTime start) :
      this(start.TimeOfDay, start.Date, new List<DayOfWeek>() { start.DayOfWeek }, null, null)
    { }

    /// <summary>
    ///   This constructor is used to create a weekly event that occurs after
    ///   the specified start date but only on the selected days of the week.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="days"></param>
    public Weekly(DateTime start, List<DayOfWeek> days) :
      this(start.TimeOfDay, start.Date, days, null, null)
    { }

    /// <summary>
    ///   This constructor is used to create a weekly event that occurs on
    ///   the same day of the week as the start date and repeats for the
    ///   specified number of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="numberOfReccurences"></param>
    public Weekly(DateTime start, int numberOfReccurences) :
      this(start.TimeOfDay, start.Date, new List<DayOfWeek>() { start.DayOfWeek }, null, numberOfReccurences)
    { }

    /// <summary>
    ///   This constructor is used to create a weekly event that occurs after
    ///   the specified start date but only on the selected days of the week
    ///   and repeats for the specified number of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="days"></param>
    /// <param name="numberOfReccurences"></param>
    public Weekly(DateTime start, List<DayOfWeek> days, int numberOfReccurences) :
      this(start.TimeOfDay, start.Date, days, null, numberOfReccurences)
    { }

    /// <summary>
    ///  This constructor is used to create a weekly event that occurs on
    ///  the same day of the week as the start date and ends on the specified
    ///  end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Weekly(DateTime start, DateTime end) :
      this(start.TimeOfDay, start.Date, new List<DayOfWeek>() { start.DayOfWeek }, end, null)
    { }

    /// <summary>
    ///   This constructor is used to create a weekly event that occurs after
    ///   the specified start date but only on the selected days of the week
    ///   and ends on the specified end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="days"></param>
    /// <param name="end"></param>
    public Weekly(DateTime start, List<DayOfWeek> days, DateTime end) :
      this(start.TimeOfDay, start.Date, days, end, null)
    { }

    private Weekly(TimeSpan timeOfDay, DateTime start, List<DayOfWeek> days, DateTime? end, int? numberOfReccurences)
    {
      TimeOfDay = timeOfDay;
      Start = start;
      Days = days;
      End = end;
      NumberOfRecurrences = numberOfReccurences;
    }

    #endregion CONSTRUCTORS

    public string Name => "Weekly";
    public TimeSpan TimeOfDay { get; private set; }
    public DateTime Start { get; private set; }
    public List<DayOfWeek> Days { get; private set; }
    public DateTime? End { get; private set; }
    public bool IsRecurring => true;

    public int? NumberOfRecurrences { get; private set; }
    public int Every { get; private set; } = 0;
    public int Skip { get; private set; } = 0;

    public IEnumerable<DateTime> TriggerDates
    {
      get
      {
        var next = GetFirstEventDate();
        var count = NumberOfRecurrences ?? int.MaxValue;

        while (next.HasValue && count > 0)
        {
          yield return next.Value;
          count--;
          next = GetNextEventDate(next);
        }
      }
    }

    public DateTime? GetNextEventDate(DateTime? lastTriggered)
    {
      // case asking for first event so make sure the last triggered is before the first event
      if (!lastTriggered.HasValue)
        lastTriggered = Start.Date.Add(TimeOfDay).AddSeconds(-1);

      // case asking for next event when there is no next event
      if (End.HasValue && lastTriggered.Value >= End.Value)
        return null;

      // If the last triggered time of day is equal or greater than the
      // time of day for the event, then increment the day becaus the
      // next event will be at least tomorrow at the same time.
      if (lastTriggered.Value.TimeOfDay >= TimeOfDay)
        lastTriggered = lastTriggered.Value.Date.AddDays(1).Add(TimeOfDay);
      else
        // Else the last triggered time of day is less than the time of day
        // for the event, so just set the time of day for the last triggered
        // to the time of day for the event.
        lastTriggered = lastTriggered.Value.Date.Add(TimeOfDay);

      // If the last triggered is a valid trigger date, then return it
      if (Days.Contains(lastTriggered.Value.DayOfWeek))
        return lastTriggered.Value;
      else
        // Else increment the day and try again.
        return GetNextEventDate(lastTriggered);
    }

    public DateTime? GetFirstEventDate() => GetNextEventDate(null);
  }
}
