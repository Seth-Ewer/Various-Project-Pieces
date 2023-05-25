namespace Paylocity.SchedulerApi.Domain
{
  public class Schedule
  {
    public Schedule(string producer, string name, Uri triggerAddress, string triggerPayload, IFrequency frequency)
    {
      Id = Guid.NewGuid();
      RequestingApp = producer;
      ScheduleOperationName = name;
      TriggerAddress = triggerAddress;
      TriggerPayload = triggerPayload;
      Frequency = frequency;
    }

    public Guid Id { get; private set; }

    // requester or requestingApp?
    public string RequestingApp { get; private set; }

    // May need some kind of concatenation of things to get a natrual key
    public string ScheduleOperationName { get; private set; }

    public IFrequency Frequency { get; private set; }

    public Uri TriggerAddress { get; private set; }

    // It could be that we don't use a POST operation when invoking the trigger and just
    // rely on the URI having all the required IDs or stuff in the query string and/or
    // in the URI path. If we remove this property, then we would need to use a GET instead
    // of a POST.
    public string TriggerPayload { get; private set; }

    //public List<TriggeredEvents> Audit { get; private set; }
  }
}
