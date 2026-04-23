using Application.Features.Accounts.Commands.InsertUser;
using Application.Features.Accounts.Commands.RegisterAccount;
using Domain.Shared;
using MassTransit;
using MassTransit.Middleware;
using MediatR;
using PasswordGenerator;
using Shared.Contracts;

namespace Infrastructure.Consumers;

public class CreateAccountConsumer : IConsumer<CreateUserDoctorAccountRequest>
{
    private readonly IMediator _mediator;

    public CreateAccountConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<CreateUserDoctorAccountRequest> context)
    {
        var message = context.Message;
        var pswSettings = new PasswordSettings(
            includeLowercase: true,
            includeUppercase: true,
            includeNumeric: true,
            includeSpecial: true,
            passwordLength: 12,
            maximumAttempts: 1000,
            usingDefaults: false
        );
        var psw = new Password(pswSettings).Next();
        var request = new InsertUserCommand(message.Id, message.Email,psw, null, AccountRole.Doctor);
        
        await _mediator.Send(request);
    }
}
