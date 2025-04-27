namespace ChatSample.CRUD.Saga.PremiumChat.Data
{
    public class PremiumChatData : ContainSagaData
    {
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }

        public bool BothAuthenticated { get; set; }
        public bool PaymentIsDone { get; set; }
    }
}
