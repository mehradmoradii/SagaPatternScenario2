using ChatSample.CRUD.Command.Messages;
using ChatSample.Infrastructures.Interfaces.Command;
using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using ChatSample.Infrastructures.Interfaces.Repository;
using ChatSample.Modules.Domains._0_Chats.Aggregate;
using ChatSample.Modules.Domains._0_Chats.Entity;
using System.Windows.Input;

namespace ChatSample.CRUD.Command.Handlers
{
    public class SendMessageCommandHandler : ICommandHandle<SendMessageCommand>
    {
        private readonly IRpcClient _client;
        private readonly IBaseRepository<Message, Guid> _messageRepository;
        private readonly IBaseRepository<Chat, Guid> _chatRepository;


        public SendMessageCommandHandler(IRpcClient client, IBaseRepository<Message, Guid> messageRepository,IBaseRepository<Chat, Guid> chatRepository)
        {
            _client = client;
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            
        }



        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var check = _client.CallWithResponse(request.recieverID, "Check-Authentication", "Result-Chack-Authentication", 60000);

            if(check.Result == "true")
            {
                var senderid = Guid.Parse(request.senderID);
                var recieverid= Guid.Parse(request.recieverID);

                var chatExistance1 = _chatRepository.FindByCondition(o=> o.Owner2 == senderid && o.Owner1 == recieverid).FirstOrDefault();
                var chatExistance2 = _chatRepository.FindByCondition(o => o.Owner1 == senderid && o.Owner2 == recieverid).FirstOrDefault();



                Chat chatToUse;

                if (chatExistance1 != null && chatExistance2 != null)
                {
                    

                    if (chatExistance1 != null)
                    {
                        chatToUse = chatExistance1;
                    }
                    else if (chatExistance2 != null)
                    {
                        chatToUse = chatExistance2;
                    }
                    else
                    {
                        chatToUse = new Chat
                        {
                            Owner1 = senderid,
                            Owner2 = recieverid,
                        };
                        await _chatRepository.Create(chatToUse);
                        await _chatRepository.Save();
                    }
                    var newmsg = new Message
                    {
                        Text = request.Text,
                        Belongto = chatToUse

                    };

                    await _messageRepository.Create(newmsg);
                    await _messageRepository.Save();

                    Task.CompletedTask.Wait();

                }

                

               

            }
        }
    }
}
