namespace Paylocity.SchedulerApi.Domain
{
  /// <summary>
  ///   This class is used to get the current date and time.  It is used to
  ///   facilitate testing because it abstracts the process of getting the
  ///   current date and time.
  /// </summary>
  public class Clock
  {
    public Clock() : this(DateTime.Now)
    {
    }

    public Clock(DateTime dateTime)
    {
      this.SetCurrentDateTime(dateTime);
    }

    public static Clock Instance { get; } = new Clock();

    private TimeSpan Offset { get; set; }

    public void SetCurrentDateTime(DateTime current)
    {
      this.Offset = current.Subtract(DateTime.Now);
    }

    public DateTime GetCurrentDate()
    {
      return DateTime.Now.Add(Offset).Date;
    }

    public DateTime GetCurrentDateTime()
    {
      return DateTime.Now.Add(Offset);
    }
  }
}
