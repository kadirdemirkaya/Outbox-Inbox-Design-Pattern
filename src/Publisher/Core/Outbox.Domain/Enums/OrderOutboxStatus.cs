namespace Outbox.Domain.Enums
{
    public enum OrderOutboxStatus
    {
        Started = 1,
        Processing = 2,
        Done = 3,
        Fail = 4
    }
}
