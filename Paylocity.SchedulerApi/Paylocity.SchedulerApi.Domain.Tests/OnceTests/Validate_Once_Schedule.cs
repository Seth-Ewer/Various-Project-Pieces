using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.OnceTests
{
  public class Validate_Once_Schedule
  {
    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Once Frequency")]
    public void Fires_Only_Once()
    {
      // Arrange
      var start = new DateTime(2023, 10, 15, 9, 0, 0);
      var expected = new DateTime(2023, 10, 15, 9, 10, 0);
      Clock.Instance.SetCurrentDateTime(start);

      var SUT = new Schedule(
        "Unit Test",
        "Validate Only Once Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Once(start.AddMinutes(10)));

      // Act
      var dates = SUT.Frequency.TriggerDates;

      // Assert
      Assert.True(dates.Count() == 1);
      AssertExtensions.DatesMatchWithinOneSecond(expected, dates.First());

      Assert.Equal("Once", SUT.Frequency.Name);
      Assert.False(SUT.Frequency.End.HasValue);
      Assert.False(SUT.Frequency.IsRecurring);
      Assert.Equal(1, SUT.Frequency.NumberOfRecurrences);
      Assert.Equal(0, SUT.Frequency.Every);
      Assert.Equal(0, SUT.Frequency.Skip);
    }
  }
}
