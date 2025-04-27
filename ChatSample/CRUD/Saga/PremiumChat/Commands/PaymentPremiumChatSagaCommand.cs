namespace ChatSample.CRUD.Saga.PremiumChat.Commands
{
    public class PaymentPremiumChatSagaCommand : ICommand
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
    }
}
