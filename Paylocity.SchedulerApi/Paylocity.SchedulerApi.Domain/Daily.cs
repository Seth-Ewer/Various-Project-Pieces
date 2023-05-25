using System.Runtime.Serialization.Formatters;

namespace Paylocity.SchedulerApi.Domain
{
  /// <summary>
  ///   This class represents a daily frequency.  It is used to calculate the
  ///   next event date for a schedule.
  /// </summary>
  public class Daily : IFrequency
  {
    #region CONSTRUCTORS

    /// <summary>
    ///   This constructor is used to create a daily event that occurs on
    ///   every day at the same time of day as the start date's time of day
    ///   with no end.
    /// </summary>
    /// <param name="start"></param>
    public Daily(DateTime start) :
      this(start.TimeOfDay, start.Date, null, null)
    { }

    /// <summary>
    ///   This constructor is used to create a daily event that occurs on
    ///   every day at the same time of day as the start date's time of day
    ///   and repeats for the specified number of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="numberOfReccurences"></param>
    public Daily(DateTime start, int numberOfReccurences) :
      this(start.TimeOfDay, start.Date, null, numberOfReccurences)
    { }

    /// <summary>
    ///   This constructor is used to create a daily event that occurs on
    ///   every EVERY days and skips each SKIP event at the same time of
    ///   day as the start date's time of day and ends repeats for the
    ///   specified number of times.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="numberOfReccurences"></param>
    public Daily(DateTime start, int numberOfReccurences, int every, int skip) :
      this(start.TimeOfDay, start.Date, null, numberOfReccurences)
    {
      Every = every;
      Skip = skip;

      if (Every < 1)
        Every = 1;
      if (Skip < 0)
        Skip = 0;
    }

    /// <summary>
    ///   This constructor is used to create a daily event that occurs on
    ///   every day at the same time of day as the start date's time of day
    ///   and ends on the specified end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Daily(DateTime start, DateTime end) :
      this(start.TimeOfDay, start.Date, end, null)
    { }

    /// <summary>
    ///   This constructor is used to create a daily event that occurs on
    ///   every EVERY days and skips each SKIP event at the same time of day as the start date's time of day
    ///   and ends on the specified end date.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Daily(DateTime start, DateTime end, int every, int skip) :
      this(start.TimeOfDay, start.Date, end, null)
    {
      Every = every;
      Skip = skip;
    }

    private Daily(TimeSpan timeOfDay, DateTime start, DateTime? end, int? numberOfReccurences)
    {
      TimeOfDay = timeOfDay;
      Start = start;
      End = end;
      NumberOfRecurrences = numberOfReccurences;
    }

    #endregion CONSTRUCTORS

    public string Name => "Daily";

    public TimeSpan TimeOfDay { get; private set; }
    public DateTime Start { get; private set; }
    public DateTime? End { get; private set; }
    public bool IsRecurring => true;
    public int? NumberOfRecurrences { get; private set; }
    public int Every { get; private set; } = 1;
    public int Skip { get; private set; } = 0;

    public IEnumerable<DateTime> TriggerDates
    {
      get
      {
        var next = GetFirstEventDate();
        var count = NumberOfRecurrences.HasValue ? NumberOfRecurrences.Value : int.MaxValue;

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
      // case asking for first event
      if (!lastTriggered.HasValue)
        return Start.Date.Add(TimeOfDay);
      
      // case asking for next event when there is no next event
      if (End.HasValue && lastTriggered.Value >= End.Value)
        return null;

      // adjusts lastTriggered to be on the day of the next potential trigger
      if (lastTriggered.Value.TimeOfDay >= TimeOfDay)
        lastTriggered = lastTriggered.Value.AddDays(1);

      // adjusts lastTriggered to the correct time of day
      lastTriggered = lastTriggered.Value.Date.Add(TimeOfDay);

      // adjusts lastTriggered to account for 'Every'
      if ((lastTriggered - Start).Value.Days % Every != 0)
        lastTriggered = lastTriggered.Value.AddDays(Every - ((lastTriggered - Start).Value.Days % Every));

      // adjusts lastTriggered to account for skipped days
      if (Skip != 0 && ((lastTriggered - Start).Value.Days+Every) % Skip*Every == 0)
        lastTriggered = lastTriggered.Value.AddDays(Every);

      // adjust lastTriggered to correct time of day, return
      return lastTriggered.Value.Date.Add(TimeOfDay);
    }

    public DateTime? GetFirstEventDate() => GetNextEventDate(null);
  }
}
