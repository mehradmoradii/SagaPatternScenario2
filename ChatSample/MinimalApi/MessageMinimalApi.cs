using ChatSample.CRUD.Command.Messages;
using ChatSample.CRUD.Saga.PremiumChat.Commands;
using ChatSample.Infrastructures.Interfaces.MinimalApi;

using MediatR;

namespace ChatSample.MinimalApi
{
    public class MessageMinimalApi : IMinimalApi
    {
        public void RegisterEndpints(WebApplication app)
        {
            app.MapPost("send/Message/", SendMessageToUser);
            app.MapPost("StartPremiumChat/", StartPremiumChat);
            
        }


        public async Task<IResult> SendMessageToUser(SendMessageCommand command, IMediator mediator)
        {
            await mediator.Send(command);

            if (Task.CompletedTask.IsCompleted)
            {
                return Results.Ok();
            }
            return Results.BadRequest();
            
        }


        public async Task<IResult> StartPremiumChat(StartPremiumChatProccessCommand command, IMessageSession session)
        {
            await session.Send(command);
            return Results.Accepted();
        }



    }
}
