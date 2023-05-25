using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.DailyTests
{
  public class Validate_Daily_Schedule_With_No_End
  {
    private readonly DateTime start;
    private readonly DateTime expectedFirst;
    private readonly Schedule SUT;

    public Validate_Daily_Schedule_With_No_End()
    {
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      expectedFirst = start;

      Clock.Instance.SetCurrentDateTime(start);
      SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Day Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Daily(start));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Daily_Schedule_Starts_But_Does_Not_Stop()
    {
      // Act
      // Get the full stream of all trigger dates for this frequency
      var dates = SUT.Frequency.TriggerDates.Take(10000);

      // Assert
      Assert.Equal(10000, dates.Count());
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst, dates.First());
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Daily_Schedule_Still_Fires_In_100_Years()
    {
      // Act
      // Get the full stream of all trigger dates for this frequency
      var date = SUT.Frequency.GetNextEventDate(Clock.Instance.GetCurrentDateTime().AddYears(100));

      // Assert
      Assert.NotNull(date);
      AssertExtensions.TimesMatchWithinOneSecond(expectedFirst.TimeOfDay, date.Value.TimeOfDay);
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Creates_Trigger_Event_On_Next_Day()
    {
      // Arrange
      var lastTriggered = new DateTime(2423, 7, 13, 2, 17, 0);
      var expected = new DateTime(2423, 7, 13, 9, 0, 0);

      // Act
      var nextDay = SUT.Frequency.GetNextEventDate(lastTriggered);
      var secondDay = SUT.Frequency.GetNextEventDate(nextDay);

      // Assert
      Assert.NotNull(nextDay);
      Assert.True(nextDay.HasValue);
      if (nextDay.HasValue)
        AssertExtensions.DatesMatchWithinOneSecond(expected, nextDay.Value);

      Assert.NotNull(secondDay);
      Assert.True(secondDay.HasValue);
      if (secondDay.HasValue)
        AssertExtensions.DatesMatchWithinOneSecond(expected.AddDays(1), secondDay.Value);
    }
  }
}
