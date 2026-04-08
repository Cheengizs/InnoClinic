using Application.Features.Accounts.Queries.GetAdminsEmails;
using MassTransit;
using MediatR;
using InnoClinic.Shared.Contracts; 

namespace Infrastructure.Consumers;

public class GetAdminEmailsConsumer : IConsumer<GetAdminEmailsRequest>
{
    private readonly IMediator _mediator;

    public GetAdminEmailsConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<GetAdminEmailsRequest> context)
    {
        var query = new GetAdminEmailsQuery(); 
        
        var result = await _mediator.Send(query, context.CancellationToken);

        if (result.IsSuccess)
        {
            await context.RespondAsync(new GetAdminEmailsResponse(result.Value ?? []));
        }
        else
        {
            throw new Exception($"Failed to get admin emails: {result.ErrorMessage}");
        }
    }
}
