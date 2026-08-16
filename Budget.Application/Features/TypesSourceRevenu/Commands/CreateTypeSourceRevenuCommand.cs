using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.TypesSourceRevenu;
using Budget.Domain.Entities;
using Mediator;

namespace Budget.Application.Features.TypesSourceRevenu.Commands;

public sealed record CreateTypeSourceRevenuCommand(string Nom) : ICommand<TypeSourceRevenuDto>;

public sealed class CreateTypeSourceRevenuCommandHandler(ITypeSourceRevenuRepository typeSourceRevenuRepository)
    : ICommandHandler<CreateTypeSourceRevenuCommand, TypeSourceRevenuDto>
{
    public async ValueTask<TypeSourceRevenuDto> Handle(CreateTypeSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var typeSourceRevenu = new TypeSourceRevenu(command.Nom);

        await typeSourceRevenuRepository.AddAsync(typeSourceRevenu, cancellationToken);

        return typeSourceRevenu.ToDto();
    }
}
