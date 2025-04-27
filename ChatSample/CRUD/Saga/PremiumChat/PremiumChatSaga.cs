using ChatSample.CRUD.Saga.PremiumChat.Commands;
using ChatSample.CRUD.Saga.PremiumChat.Data;
using ChatSample.CRUD.Saga.PremiumChat.Events;
using MediatR;

namespace ChatSample.CRUD.Saga.PremiumChat
{
    public class PremiumChatSaga : Saga<PremiumChatData>,
                                    IAmStartedByMessages<StartPremiumChatProccessCommand>,
                                    IHandleMessages<AuthenticationCheckedSagaEvent>,
                                    IHandleMessages<PaymentPremiumChatSagaEvent>

    {

        



        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PremiumChatData> mapper)
        {
            mapper.MapSaga(saga => saga.SenderId)
                .ToMessage<StartPremiumChatProccessCommand>(msg => Guid.Parse(msg.SenderId));

            mapper.ConfigureMapping<AuthenticationCheckedSagaEvent>(msg => msg.SenderId.ToString())
                .ToSaga(msg => msg.SenderId);

            mapper.ConfigureMapping<PaymentPremiumChatSagaEvent>(msg => msg.SenderId.ToString())
                .ToSaga(msg => msg.SenderId);
        }



        public async Task Handle(StartPremiumChatProccessCommand message, IMessageHandlerContext context)
        {
            Data.SenderId = Guid.Parse(message.SenderId);
            Data.RecieverId = Guid.Parse(message.RecieverId);

            var msg = new CheckAuthPremiumChatSagaCommand
            {
                SenderId = message.SenderId,
                RecieverId = message.RecieverId,
            };



            await context.Send(msg);
            
        }

        public async Task Handle(AuthenticationCheckedSagaEvent message, IMessageHandlerContext context)
        {
            if (!message.SenderIsAuthenticated && !message.RecieverIsAuthenticated)
            {
                await context.Publish(new FailedEvent
                {
                    SenderId = message.SenderId,
                    Reason = "both Users Are not authenticated"
                });

            }

            if (message.SenderIsAuthenticated && !message.RecieverIsAuthenticated)
            {
                await context.Publish(new FailedEvent
                {
                    SenderId = message.SenderId,
                    Reason = "your friend is not authenticated"
                });

            }
            if (!message.SenderIsAuthenticated && message.RecieverIsAuthenticated)
            {
                await context.Publish(new FailedEvent
                {
                    SenderId = message.SenderId,
                    Reason = "you are not authenticated"
                });

            }

            Data.BothAuthenticated = true;

            var nextStep = new PaymentPremiumChatSagaCommand
            {
                SenderId = message.SenderId,
                RecieverId = message.RecieverId

            };

            await context.Send(nextStep);



        }




        public async Task Handle(PaymentPremiumChatSagaEvent message, IMessageHandlerContext context)
        {
            if (!message.PaymentIsDone)
            {
                await context.Publish(new FailedEvent
                {
                    SenderId = message.SenderId,
                    Reason = "there is something wrong in your payment"
                });

                MarkAsComplete();
            }
            else
            {

                Data.PaymentIsDone = true;
                await context.SendLocal(new CreatePremiumChatSagaCommand
                {
                    SenderId = message.SenderId,
                    RecieverId = message.RecieverId
                });

                MarkAsComplete();
            }
        }
    }
}
