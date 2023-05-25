using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.WeeklyTests
{
  public class Validate_Monthly_Schedule_With_No_End
  {
    private readonly DateTime start;
    private readonly Schedule SUT;

    public Validate_Monthly_Schedule_With_No_End()
    {
      // Arrange
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      Clock.Instance.SetCurrentDateTime(start.AddMinutes(-10));

      SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Month Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Monthly(start));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Fires_Once_Per_Month_Forever()
    {
      // Act
      var dates = SUT.Frequency.TriggerDates.Skip(10000).Take(10).ToList();

      // Assert
      Assert.Equal(10, dates.Count);
      foreach (var date in dates)
      {
        Assert.Equal(start.Day, date.Day);
        AssertExtensions.TimesMatchWithinOneSecond(start.TimeOfDay, date.TimeOfDay);
      }
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Monthly Frequency")]
    public void Can_Get_First_Event_Date()
    {
      // Act
      var firstEvent = SUT.Frequency.GetFirstEventDate();
      var getNext = SUT.Frequency.GetNextEventDate(start.AddDays(-3));
      var dotFirst = SUT.Frequency.TriggerDates.First();

      // Assert
      Assert.NotNull(firstEvent);
      Assert.NotNull(getNext);
      if (firstEvent.HasValue)
        AssertExtensions.DatesMatchWithinOneSecond(start, firstEvent.Value);
      if (getNext.HasValue)
        AssertExtensions.DatesMatchWithinOneSecond(start, getNext.Value);
      AssertExtensions.DatesMatchWithinOneSecond(start, dotFirst);
    }
  }
}
