namespace ChatSample.CRUD.Saga.PremiumChat.Events
{
    public class PaymentPremiumChatSagaEvent : IEvent
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

        public bool PaymentIsDone { get; set; }
    }
}
