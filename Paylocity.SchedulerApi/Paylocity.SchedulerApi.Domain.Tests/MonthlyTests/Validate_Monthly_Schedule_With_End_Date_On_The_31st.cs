using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.WeeklyTests
{
  public class Validate_Monthly_Schedule_With_End_Date_At_End_Of_Month
  {
    private readonly DateTime startOn30th;
    private readonly DateTime startOn31st;
    private readonly DateTime end;
    private readonly DateTime distantFebruary;

    private Schedule SUT;

    public Validate_Monthly_Schedule_With_End_Date_At_End_Of_Month()
    {
      // Arrange
      startOn30th = new DateTime(2023, 10, 30, 9, 0, 0);
      startOn31st = new DateTime(2023, 10, 31, 9, 0, 0);
      distantFebruary = new DateTime(2487, 2, 25, 9, 0, 0);
      end = startOn30th.AddMonths(100);
      Clock.Instance.SetCurrentDateTime(startOn30th.AddMinutes(-10));

      SUT = new Schedule(
        "Unit Test",
        "Validate Monthly Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Monthly(startOn30th, end));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Fires_Once_Per_Month_For_100_Months_On_30th()
    {
      // Act
      var first = SUT.Frequency.TriggerDates.First();
      var last = SUT.Frequency.TriggerDates.Last();
      var dates = SUT.Frequency.TriggerDates.ToList();

      // Assert
      AssertExtensions.DatesMatchWithinOneSecond(startOn30th, first);
      AssertExtensions.DatesMatchWithinOneSecond(end, last);
      Assert.Equal(101, dates.Count);

      foreach (var date in dates)
      {
        if (DateTime.DaysInMonth(date.Year, date.Month) == 31)
        {
          Assert.Equal(30, date.Day);
        }
        else
        {
          Assert.Equal(DateTime.DaysInMonth(date.Year, date.Month), date.Day);
        }
        AssertExtensions.TimesMatchWithinOneSecond(startOn30th.TimeOfDay, date.TimeOfDay);
      }
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Fires_Once_Per_Month_For_100_Months_On_31st()
    {
      // Arrange
      SUT = new Schedule(
        "Unit Test",
        "Validate Monthly Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Monthly(startOn31st, end));

      // Act
      var first = SUT.Frequency.TriggerDates.First();
      var last = SUT.Frequency.TriggerDates.Last();
      var dates = SUT.Frequency.TriggerDates.ToList();

      // Assert
      AssertExtensions.DatesMatchWithinOneSecond(startOn31st, first);
      AssertExtensions.DatesMatchWithinOneSecond(end, last);
      Assert.Equal(101, dates.Count);

      foreach (var date in dates)
      {
        Assert.Equal(DateTime.DaysInMonth(date.Year, date.Month), date.Day);
        AssertExtensions.TimesMatchWithinOneSecond(startOn31st.TimeOfDay, date.TimeOfDay);
      }
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Does_Not_Pass_End_Date()
    {
      // Act
      // There should be no more trigger dates
      var nextIsNull = SUT.Frequency.GetNextEventDate(distantFebruary);

      // Assert
      Assert.Null(nextIsNull);
    }
  }
}
