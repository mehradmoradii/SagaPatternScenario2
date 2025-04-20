using ChatSample.Infrastructures.Interfaces.Command;

namespace ChatSample.CRUD.Command.Messages
{
    public class SendMessageCommand : ICommand
    {
        public string senderID { get; set; }
        public string recieverID { get; set; }
        public string Text { get; set; }

    }
}
