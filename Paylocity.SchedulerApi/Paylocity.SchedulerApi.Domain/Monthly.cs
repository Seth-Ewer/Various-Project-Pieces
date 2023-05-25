namespace Paylocity.SchedulerApi.Domain
{
  public class Monthly : IFrequency
  {
    #region CONSTRUCTORS

    /// <summary>
    ///   This constructor is used to create a weekly event that occurs on
    ///   on the same day as the start date and at the same time of day as
    ///   the start date's time of day with no end.
    /// </summary>
    /// <param name="start"></param>
    public Monthly(DateTime start) :
      this(start.TimeOfDay, (short)start.Day, start.Date, null, null)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   on the given day at the same time of day as the start date's time of day
    ///   with no end.
    /// </summary>
    /// <param name="start"></param>
    public Monthly(DateTime start, short day) :
      this(start.TimeOfDay, day, start.Date, null, null)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   on the given day at the given time of day with no end.
    /// </summary>
    /// <param name="start"></param>
    public Monthly(DateTime start, TimeSpan timeOfDay, short day) :
      this(timeOfDay, day, start.Date, null, null)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   every day at the same time of day as the start date's time of day
    ///   and repeats for the specified number of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="numberOfReccurences"></param>
    public Monthly(DateTime start, int numberOfReccurences) :
      this(start.TimeOfDay, (short)start.Day, start.Date, null, numberOfReccurences)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   on the specified day of the month and at the same time of day as
    ///   the start date's time of day and repeats for the specified number
    ///   of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="numberOfReccurences"></param>
    public Monthly(DateTime start, short day, int numberOfReccurences) :
      this(start.TimeOfDay, day, start.Date, null, numberOfReccurences)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   on the specified day of the month and at the specified time of day
    ///   and repeats for the specified number of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="numberOfReccurences"></param>
    public Monthly(DateTime start, TimeSpan timeOfDay, short day, int numberOfReccurences) :
      this(timeOfDay, day, start.Date, null, numberOfReccurences)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   every month on the same day as the start date and at the same time
    ///   of day as the start date's time of day and ends on the specified
    ///   end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Monthly(DateTime start, DateTime end) :
      this(start.TimeOfDay, (short)start.Day, start.Date, end, null)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   every month on the specified day and at the same time of day as
    ///   the start date's time of day and ends on the specified end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Monthly(DateTime start, short day, DateTime end) :
      this(start.TimeOfDay, day, start.Date, end, null)
    { }

    /// <summary>
    ///   This constructor is used to create a monthly event that occurs on
    ///   every month on the specified day at the specified time of day and
    ///   ends on the specified end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Monthly(DateTime start, TimeSpan timeOfDay, short day, DateTime end) :
      this(timeOfDay, day, start.Date, end, null)
    { }

    private Monthly(TimeSpan timeOfDay, short day, DateTime start, DateTime? end, int? numberOfReccurences)
    {
      TimeOfDay = timeOfDay;
      Day = day;
      Start = start;
      End = end;
      NumberOfRecurrences = numberOfReccurences;
    }

    #endregion CONSTRUCTORS

    public string Name => "Monthly";

    public TimeSpan TimeOfDay { get; private set; }

    public DateTime Start { get; private set; }

    public DateTime? End { get; private set; }

    public short Day { get; private set; }

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

    public DateTime? GetFirstEventDate() => GetNextEventDate(null);

    public DateTime? GetNextEventDate(DateTime? lastTriggered)
    {
      // case asking for first event
      if (!lastTriggered.HasValue)
        return new DateTime(Start.Year, Start.Month, DayOfMonth(Start)).Add(TimeOfDay);

      // case asking for next event when there is no next event
      if (End.HasValue && lastTriggered.Value >= End.Value)
        return null;

      // if the last triggered date is before the Day, then we need
      // to trigger on the Day of the month
      if (lastTriggered.Value.Day < DayOfMonth(lastTriggered.Value))
      {
        lastTriggered = new DateTime(
          lastTriggered.Value.Year,
          lastTriggered.Value.Month,
          DayOfMonth(lastTriggered.Value));
        return lastTriggered.Value.Add(TimeOfDay);
      }
      else
      {
        lastTriggered = lastTriggered.Value.AddMonths(1);
        lastTriggered = new DateTime(
          lastTriggered.Value.Year,
          lastTriggered.Value.Month,
          DayOfMonth(lastTriggered.Value));

        return lastTriggered.Value.Add(TimeOfDay);
      }
    }

    private int DayOfMonth(DateTime lastTriggered)
    {
      return Day > DateTime.DaysInMonth(lastTriggered.Year, lastTriggered.Month)
        ? DateTime.DaysInMonth(lastTriggered.Year, lastTriggered.Month)
        : Day;
    }
  }
}
