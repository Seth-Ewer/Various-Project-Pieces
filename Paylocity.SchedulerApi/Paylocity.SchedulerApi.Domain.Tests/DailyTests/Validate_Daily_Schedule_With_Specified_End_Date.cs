using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.DailyTests
{
  public class Validate_Daily_Schedule_With_Specified_End_Date
  {
    private readonly DateTime start;
    private readonly DateTime expectedFirst;
    private readonly DateTime expectedThird;
    private readonly DateTime expectedLast;

    private readonly Schedule SUT;

    public Validate_Daily_Schedule_With_Specified_End_Date()
    {
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      expectedFirst = start;
      expectedThird = start.AddDays(3);
      expectedLast = start.AddDays(10);

      Clock.Instance.SetCurrentDateTime(start.AddMinutes(-10));
      SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Day Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Daily(start, expectedLast));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Creates_Trigger_Event_On_Thrid_Day()
    {
      // Act
      var nextDay = SUT.Frequency.GetNextEventDate(start.AddDays(3).AddMinutes(-5));

      // Assert
      Assert.NotNull(nextDay);
      Assert.True(nextDay.HasValue);
      if (nextDay.HasValue)
        AssertExtensions.DatesMatchWithinOneSecond(expectedThird, nextDay.Value);
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Creates_Trigger_Event_On_First_Day()
    {
      // Act
      var nextOnSameDay = SUT.Frequency.GetNextEventDate(start.AddHours(-2));

      // Assert
      Assert.NotNull(nextOnSameDay);
      Assert.True(nextOnSameDay.HasValue);
      if (nextOnSameDay.HasValue)
        AssertExtensions.DatesMatchWithinOneSecond(expectedFirst, nextOnSameDay.Value);
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
      Assert.Equal(11, dates.Count());
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst, dates.First());
      AssertExtensions.DatesMatchWithinOneSecond(expectedLast, dates.Last());
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Daily_Schedule_Does_Not_Pass_End_Date()
    {
      // Act
      // There should be no more trigger dates
      var nextIsNull = SUT.Frequency.GetNextEventDate(
        Clock.Instance.GetCurrentDateTime().AddDays(20));

      // Assert
      Assert.Null(nextIsNull);
    }
  }
}
