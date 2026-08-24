using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Devises;
using Budget.Domain.Entities;
using Mediator;

namespace Budget.Application.Features.Devises.Commands;

public sealed record CreateDeviseCommand(string Nom) : ICommand<DeviseDto>;

public sealed class CreateDeviseCommandHandler(IDeviseRepository deviseRepository)
    : ICommandHandler<CreateDeviseCommand, DeviseDto>
{
    public async ValueTask<DeviseDto> Handle(CreateDeviseCommand command, CancellationToken cancellationToken)
    {
        var devise = new Devise(command.Nom);

        await deviseRepository.AddAsync(devise, cancellationToken);

        return devise.ToDto();
    }
}
