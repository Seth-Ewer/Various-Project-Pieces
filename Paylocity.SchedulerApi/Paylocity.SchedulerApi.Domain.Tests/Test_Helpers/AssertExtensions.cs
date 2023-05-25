namespace Paylocity.SchedulerApi.Domain.Tests.Test_Helpers
{
  public static class AssertExtensions
  {
    public static void DatesMatchWithinOneSecond(DateTime expected, DateTime actual)
    {
      Assert.True(Math.Abs(expected.Subtract(actual).TotalSeconds) <= 1);
    }

    public static void TimesMatchWithinOneSecond(TimeSpan expected, TimeSpan actual)
    {
      Assert.True(Math.Abs(expected.Subtract(actual).TotalSeconds) <= 1);
    }
  }
}
