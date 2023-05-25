using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.WeeklyTests
{
  public class Validate_Monthly_Schedule_With_End_Date
  {
    private readonly DateTime start;
    private readonly DateTime end;
    private readonly Schedule SUT;

    public Validate_Monthly_Schedule_With_End_Date()
    {
      // Arrange
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      end = start.AddMonths(100);
      Clock.Instance.SetCurrentDateTime(start.AddMinutes(-10));

      SUT = new Schedule(
        "Unit Test",
        "Validate Monthly Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Monthly(start, end));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Fires_Once_Per_Month_For_100_Months()
    {
      // Act
      var first = SUT.Frequency.TriggerDates.First();
      var last = SUT.Frequency.TriggerDates.Last();
      var dates = SUT.Frequency.TriggerDates.ToList();

      // Assert
      AssertExtensions.DatesMatchWithinOneSecond(start, first);
      AssertExtensions.DatesMatchWithinOneSecond(end, last);
      Assert.Equal(101, dates.Count);

      foreach (var date in dates)
      {
        Assert.Equal(start.Day, date.Day);
        AssertExtensions.TimesMatchWithinOneSecond(start.TimeOfDay, date.TimeOfDay);
      }
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Does_Not_Pass_End_Date()
    {
      // Act
      // There should be no more trigger dates
      var nextIsNull = SUT.Frequency.GetNextEventDate(start.AddMonths(200));

      // Assert
      Assert.Null(nextIsNull);
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Has_Default_Property_Values()
    {
      Assert.Equal("Monthly", SUT.Frequency.Name);
      Assert.True(SUT.Frequency.IsRecurring);
      Assert.Equal(0, SUT.Frequency.Every);
      Assert.Equal(0, SUT.Frequency.Skip);
    }
  }
}
