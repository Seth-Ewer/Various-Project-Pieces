using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.DailyTests
{
  public class Validate_Daily_Schedule_With_Numbered_End
  {
    private readonly DateTime start;
    private readonly DateTime expectedFirst;
    private readonly DateTime expectedLast;
    private readonly Schedule SUT;

    public Validate_Daily_Schedule_With_Numbered_End()
    {
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      expectedFirst = start;
      expectedLast = start.AddDays(99);

      Clock.Instance.SetCurrentDateTime(start);
      SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Day Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Daily(start, 100));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Dail_Schedule_Starts_And_Stops_On_Boundaries()
    {
      // Act
      // Get the full stream of all trigger dates for this frequency
      var dates = SUT.Frequency.TriggerDates;

      // Assert
      Assert.Equal(100, dates.Count());
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst, dates.First());
      AssertExtensions.DatesMatchWithinOneSecond(expectedLast, dates.Last());
    }
  }
}
