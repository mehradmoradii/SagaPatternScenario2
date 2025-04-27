namespace ChatSample.CRUD.Saga.PremiumChat.Commands
{
    public class CheckAuthPremiumChatSagaCommand : ICommand
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
    }
}
