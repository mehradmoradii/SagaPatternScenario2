namespace ChatSample.CRUD.Saga.PremiumChat.Events
{
    public class AuthenticationCheckedSagaEvent : IEvent
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

        public bool SenderIsAuthenticated { get; set; }
        public bool RecieverIsAuthenticated { get; set; }
    }
}
