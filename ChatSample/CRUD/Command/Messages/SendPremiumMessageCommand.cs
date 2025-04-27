namespace ChatSample.CRUD.Command.Messages
{
    public class SendPremiumMessageCommand : ICommand
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string MessageContent { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}
