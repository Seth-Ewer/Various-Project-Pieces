using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.WeeklyTests
{
  public class Validate_Weekly_Schedule_With_End_Date
  {
    private readonly DateTime start;
    private readonly DateTime end;
    private readonly Schedule SUT;

    public Validate_Weekly_Schedule_With_End_Date()
    {
      // Arrange
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      end = start.AddDays(7 * 10);
      Clock.Instance.SetCurrentDateTime(start.AddMinutes(-10));

      SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Week Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Weekly(start, end));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Weekly Frequency")]
    public void Fires_Once_Per_Week_For_10_Weeks()
    {
      // Act
      var first = SUT.Frequency.TriggerDates.First();
      var last = SUT.Frequency.TriggerDates.Last();
      var dates = SUT.Frequency.TriggerDates.ToList();

      // Assert
      AssertExtensions.DatesMatchWithinOneSecond(start, first);
      AssertExtensions.DatesMatchWithinOneSecond(end, last);
      Assert.Equal(11, dates.Count());

      foreach (var date in dates)
      {
        Assert.Equal(start.DayOfWeek, date.DayOfWeek);
        AssertExtensions.TimesMatchWithinOneSecond(start.TimeOfDay, date.TimeOfDay);
      }
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Weekly Frequency")]
    public void Does_Not_Pass_End_Date()
    {
      // Act
      // There should be no more trigger dates
      var nextIsNull = SUT.Frequency.GetNextEventDate(start.AddDays(12 * 7));

      // Assert
      Assert.Null(nextIsNull);
    }
  }
}
