using ChatSample.Infrastructures.Interfaces.Command;

namespace ChatSample.CRUD.Command.Messages
{
    public class CreatePremiumChatCommand : Infrastructures.Interfaces.Command.ICommand
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
    }
}
