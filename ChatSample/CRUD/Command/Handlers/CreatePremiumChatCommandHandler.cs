using ChatSample.CRUD.Command.Messages;
using ChatSample.Infrastructures.Interfaces.Command;
using ChatSample.Modules.Domains._0_Chats.Aggregate;
using ChatSample.Repository;

namespace ChatSample.CRUD.Command.Handlers
{
    public class CreatePremiumChatCommandHandler : ICommandHandle<CreatePremiumChatCommand>
    {
        private readonly BaseRepostory<Chat,Guid> _chatRepository;

        public CreatePremiumChatCommandHandler(BaseRepostory<Chat, Guid> chatRepository)
        {
            _chatRepository = chatRepository;
            
        }


        public async Task Handle(CreatePremiumChatCommand request, CancellationToken cancellationToken)
        {
            var chat = new Chat
            {
                Owner1 = Guid.Parse(request.SenderId),
                Owner2 = Guid.Parse(request.RecieverId),

            };
            await _chatRepository.Create(chat);
            await _chatRepository.Save();
        }
    }
}
