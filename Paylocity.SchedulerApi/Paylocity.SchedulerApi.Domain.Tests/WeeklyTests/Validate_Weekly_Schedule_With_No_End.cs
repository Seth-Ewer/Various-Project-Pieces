using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.WeeklyTests
{
  public class Validate_Weekly_Schedule_With_No_End
  {
    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Weekly Frequency")]
    public void Fires_Once_Per_Week_Forever()
    {
      // Arrange
      var start = new DateTime(2023, 10, 15, 9, 0, 0);
      Clock.Instance.SetCurrentDateTime(start.AddMinutes(-10));

      var SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Week Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Weekly(start));

      // Act
      var dates = SUT.Frequency.TriggerDates.Skip(10000).Take(10).ToList();

      // Assert
      Assert.Equal(10, dates.Count());
      foreach (var date in dates)
      {
        Assert.Equal(start.DayOfWeek, date.DayOfWeek);
        AssertExtensions.TimesMatchWithinOneSecond(start.TimeOfDay, date.TimeOfDay);
      }
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Weekly Frequency")]
    public void Fires_Three_Times_Per_Week_Forever()
    {
      // Arrange
      var start = new DateTime(2023, 10, 15, 9, 0, 0);
      var days = new List<DayOfWeek>()
      {
        DayOfWeek.Monday,
        DayOfWeek.Wednesday,
        DayOfWeek.Friday
      };
      var expectedDay = DayOfWeek.Monday;
      var skipWeeks = 1337;
      var eventsPerWeek = 3;
      var numberOfEventsToGet = 100;
      Clock.Instance.SetCurrentDateTime(start.AddMinutes(-10));

      var SUT = new Schedule(
        "Unit Test",
        "Validate Three Times Per Week Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Weekly(start, days));

      // Act
      var dates = SUT.Frequency.TriggerDates
                               .Skip(skipWeeks * eventsPerWeek)
                               .Take(numberOfEventsToGet)
                               .ToList();

      // Assert
      Assert.Equal(numberOfEventsToGet, dates.Count());

      foreach (var date in dates)
      {
        Assert.Equal(expectedDay, date.DayOfWeek);
        AssertExtensions.TimesMatchWithinOneSecond(start.TimeOfDay, date.TimeOfDay);

        switch (expectedDay)
        {
          case DayOfWeek.Monday:
            expectedDay = DayOfWeek.Wednesday;
            break;

          case DayOfWeek.Wednesday:
            expectedDay = DayOfWeek.Friday;
            break;

          case DayOfWeek.Friday:
            expectedDay = DayOfWeek.Monday;
            break;
        }
      }
    }
  }
}
