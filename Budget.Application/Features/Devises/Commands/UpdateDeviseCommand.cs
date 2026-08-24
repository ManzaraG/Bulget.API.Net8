using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Devises;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Devises.Commands;

public sealed record UpdateDeviseCommand(Guid Id, string Nom) : ICommand<DeviseDto>;

public sealed class UpdateDeviseCommandHandler(IDeviseRepository deviseRepository)
    : ICommandHandler<UpdateDeviseCommand, DeviseDto>
{
    public async ValueTask<DeviseDto> Handle(UpdateDeviseCommand command, CancellationToken cancellationToken)
    {
        var devise = await deviseRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Devise), command.Id);

        devise.Renommer(command.Nom);

        await deviseRepository.UpdateAsync(devise, cancellationToken);

        return devise.ToDto();
    }
}
