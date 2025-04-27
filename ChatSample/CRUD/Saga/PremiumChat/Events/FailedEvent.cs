namespace ChatSample.CRUD.Saga.PremiumChat.Events
{
    public class FailedEvent : IEvent
    {
        public string SenderId { get; set; }
        public string Reason { get; set; }
    }
}
