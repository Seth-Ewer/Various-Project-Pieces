namespace Paylocity.SchedulerApi.Domain
{
  /// <summary>
  ///   This interface represents a frequency.  It is used to calculate the
  ///   next event date for a schedule.
  /// </summary>
  public interface IFrequency
  {
    string Name { get; }
    TimeSpan TimeOfDay { get; }
    DateTime Start { get; }
    DateTime? End { get; }
    bool IsRecurring { get; }
    int? NumberOfRecurrences { get; }

    /// <summary>
    ///   Tells the schedule to only trigger once every so
    ///   many days instead of triggering every day. A value
    ///   of less than or equal to 1 means every day.
    /// </summary>
    public int Every { get; }

    /// <summary>
    ///   Tells the schedule to skip every Nth occurance of the triggered event. A value
    ///   of zero means do not skip any.
    /// </summary>
    /// <example>
    ///   As an example, if we put in the Skip value of 3 for a daily frequency then the
    ///   schedule would trigger two times then skip the thrid. So, it would be: Sunday,
    ///   Monday, Wednesday, Thrusday, Satruday ...
    /// </example>
    /// <example>
    ///   As an example, if we put in the Every value of 2 for a daily frequency and the
    ///   skip value of 3 then the schedule would trigger every other day and skip every
    ///   thrid time it should trigger. So, it would be: Sunday, Tuesday, Saturday,
    ///   Monday, Friday...
    /// </example>
    public int Skip { get; }

    /// <summary>
    ///   This property returns a list of dates that the event will occur on.
    ///   Be sure to constrain the Linq statements or loops you use to process
    ///   this enumerable because some Frequencies have no end date and this
    ///   can be an infinite list.
    /// </summary>
    IEnumerable<DateTime> TriggerDates { get; }

    /// <summary>
    ///   This method returns the next event date after the specified date.
    /// </summary>
    /// <param name="lastTriggered"></param>
    /// <returns>Next Event Date</returns>
    DateTime? GetNextEventDate(DateTime? lastTriggered);

    /// <summary>
    ///   This method returns the first event date.
    /// </summary>
    /// <returns>First Event Date</returns>
    DateTime? GetFirstEventDate();
  }
}
