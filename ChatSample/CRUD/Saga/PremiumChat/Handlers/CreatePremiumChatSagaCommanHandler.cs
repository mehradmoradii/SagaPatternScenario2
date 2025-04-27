using ChatSample.CRUD.Saga.PremiumChat.Commands;
using ChatSample.Modules.Domains._0_Chats.Aggregate;
using ChatSample.Repository;

namespace ChatSample.CRUD.Saga.PremiumChat.Handlers
{
    public class CreatePremiumChatSagaCommanHandler : IHandleMessages<CreatePremiumChatSagaCommand>
    {
        private readonly BaseRepostory<Chat, Guid> _repostory;

        public CreatePremiumChatSagaCommanHandler(BaseRepostory<Chat, Guid> repostory)
        {
            _repostory = repostory;
        }


        public async Task Handle(CreatePremiumChatSagaCommand message, IMessageHandlerContext context)
        {
            var newChat = new Chat()
            {
                Owner1 = Guid.Parse(message.SenderId),
                Owner2 = Guid.Parse(message.SenderId),
            };

            await _repostory.Create(newChat);
            await _repostory.Save();
        }
    }
}
