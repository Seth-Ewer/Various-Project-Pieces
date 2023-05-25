using Paylocity.SchedulerApi.Domain.Tests.Test_Helpers;

namespace Paylocity.SchedulerApi.Domain.Tests.DailyTests
{
  public class Validate_Daily_Schedule_With_Skip_Days
  {
    private readonly DateTime start;
    private readonly DateTime expectedFirst;
    private readonly Schedule SUT;

    public Validate_Daily_Schedule_With_Skip_Days()
    {
      start = new DateTime(2023, 10, 15, 9, 0, 0);
      expectedFirst = start;

      Clock.Instance.SetCurrentDateTime(start);
      SUT = new Schedule(
        "Unit Test",
        "Validate Once Per Day Schedule",
        new Uri("http://localhost"),
        "Payload",
        new Daily(start, 100, 3, 5));
    }

    [Fact]
    [Trait("JIRA", "SYGSD-4037")]
    [Trait("FEATURE", "Daily Frequency")]
    public void Dail_Schedule_Starts_And_Stops_On_Boundaries()
    {
      // Act
      // Get the full stream of all trigger dates for this frequency
      var dates = SUT.Frequency.TriggerDates.ToArray();

      // Assert
      /*
      We expect that every third day and skipping the fifth trigger
      that we should get this:

      | Position | Day        | Is it a Third day? | Is it a Fifth trigger? | Included? |
      | -------- | ---------- | ------------------ | ---------------------- | --------- |
      | 00       | 10/15/2032 | include first      | no - 1                 | yes       |
      | 01       | 10/16/2032 | no                 | no                     | no        |
      | 02       | 10/17/2032 | no                 | no                     | no        |
      | 03       | 10/18/2032 | yes                | no - 2                 | yes       |
      | 04       | 10/19/2032 | no                 | no                     | no        |
      | 05       | 10/20/2032 | no                 | no                     | no        |
      | 06       | 10/21/2032 | yes                | no - 3                 | yes       |
      | 07       | 10/22/2032 | no                 | no                     | no        |
      | 08       | 10/23/2032 | no                 | no                     | no        |
      | 09       | 10/24/2032 | yes                | no - 4                 | yes       |
      | 10       | 10/25/2032 | no                 | no                     | no        |
      | 11       | 10/26/2032 | no                 | no                     | no        |
      | 12       | 10/27/2032 | yes                | yes                    | no        |
      | 13       | 10/28/2032 | no                 | no                     | no        |
      | 14       | 10/29/2032 | no                 | no                     | no        |
      | 15       | 10/30/2032 | yes                | no - 1                 | yes       |
      | 16       | 10/31/2032 | no                 | no                     | no        |
      | 17       | 11/01/2032 | no                 | no                     | no        |
      | 18       | 11/02/2032 | yes                | no - 2                 | yes       |
      | 19       | 11/03/2032 | no                 | no                     | no        |
      | 20       | 11/04/2032 | no                 | no                     | no        |
      | 21       | 11/05/2032 | yes                | no - 3                 | yes       |
      | 22       | 11/06/2032 | no                 | no                     | no        |
      | 23       | 11/07/2032 | no                 | no                     | no        |
      | 24       | 11/08/2032 | yes                | no - 4                 | yes       |
      | 25       | 11/09/2032 | no                 | no                     | no        |
      | 26       | 11/10/2032 | no                 | no                     | no        |
      | 27       | 11/11/2032 | yes                | yes                    | no        |
      | 28       | 11/12/2032 | no                 | no                     | no        |
      | 29       | 11/13/2032 | no                 | no                     | no        |
      | 30       | 11/14/2032 | yes                | no - 1                 | yes       |
      | 31       | 11/15/2032 | no                 | no                     | no        |
      | 32       | 11/16/2032 | no                 | no                     | no        |
      | 33       | 11/17/2032 | yes                | no - 2                 | yes       |
      | 34       | 11/18/2032 | no                 | no                     | no        |
      | 35       | 11/19/2032 | no                 | no                     | no        |

      */

      Assert.Equal(100, dates.Length);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(00), dates[0]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(03), dates[1]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(06), dates[2]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(09), dates[3]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(15), dates[4]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(18), dates[5]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(21), dates[6]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(24), dates[7]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(30), dates[8]);
      AssertExtensions.DatesMatchWithinOneSecond(expectedFirst.AddDays(33), dates[9]);
    }
  }
}
