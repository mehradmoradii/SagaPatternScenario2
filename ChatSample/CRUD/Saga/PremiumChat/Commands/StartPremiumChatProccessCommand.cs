namespace ChatSample.CRUD.Saga.PremiumChat.Commands
{
    public class StartPremiumChatProccessCommand : ICommand
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

       
    }
}
