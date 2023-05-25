using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.WeeklyTests
{
  public class Validate_Monthly_Schedule_With_Numbered_End
  {
    private readonly DateTime start = new DateTime(2023, 10, 15, 9, 0, 0);
    private readonly DateTime expectedFirst;
    private readonly DateTime expectedLast;

    private readonly Schedule SUT;
    private readonly int numberOfEvents = 100;

    public Validate_Monthly_Schedule_With_Numbered_End()
    {
      // First event will be on the Tuesday after the startOn30th date
      expectedFirst = start;

      // we divide the number of weeks by the number of days per week,
      // multiply by 7 days per week, then subtract one day since the
      // last one will be on Saturday.
      expectedLast = start.AddMonths(numberOfEvents - 1);
      //
      // Clock.Instance.SetCurrentDateTime(startOn30th);
      //
      SUT = new Schedule(
        "Unit Test",
        "Validate Monthly With Specified Number Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Monthly(start, numberOfEvents));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Starts_And_Stops_On_Boundaries()
    {
      // Act
      // Get the full stream of all trigger dates for this frequency
      var dates = SUT.Frequency.TriggerDates.ToList();
      var first = dates.First();
      var last = dates.Last();

      // Assert
      Assert.Equal(numberOfEvents, dates.Count);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst, first);
      AssertExtensions.DatesMatchWithinOneSecond(expectedLast, last);

      foreach (var date in dates)
      {
        Assert.Equal(start.Day, date.Day);
        AssertExtensions.TimesMatchWithinOneSecond(start.TimeOfDay, date.TimeOfDay);
      }
    }
  }
}
