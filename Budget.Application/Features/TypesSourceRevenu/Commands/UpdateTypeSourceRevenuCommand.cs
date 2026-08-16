using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.TypesSourceRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.TypesSourceRevenu.Commands;

public sealed record UpdateTypeSourceRevenuCommand(Guid Id, string Nom) : ICommand<TypeSourceRevenuDto>;

public sealed class UpdateTypeSourceRevenuCommandHandler(ITypeSourceRevenuRepository typeSourceRevenuRepository)
    : ICommandHandler<UpdateTypeSourceRevenuCommand, TypeSourceRevenuDto>
{
    public async ValueTask<TypeSourceRevenuDto> Handle(UpdateTypeSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var typeSourceRevenu = await typeSourceRevenuRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(TypeSourceRevenu), command.Id);

        typeSourceRevenu.Renommer(command.Nom);

        await typeSourceRevenuRepository.UpdateAsync(typeSourceRevenu, cancellationToken);

        return typeSourceRevenu.ToDto();
    }
}
