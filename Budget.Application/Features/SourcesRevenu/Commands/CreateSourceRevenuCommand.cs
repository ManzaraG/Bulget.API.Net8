using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Commands;

public sealed record CreateSourceRevenuCommand(string Nom, TypeSourceRevenuDto Type) : ICommand<SourceRevenuDto>;

public sealed class CreateSourceRevenuCommandHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<CreateSourceRevenuCommand, SourceRevenuDto>
{
    public async ValueTask<SourceRevenuDto> Handle(CreateSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var sourceRevenu = new SourceRevenu(command.Nom, command.Type.ToDomain(), utilisateurId);

        await sourceRevenuRepository.AddAsync(sourceRevenu, cancellationToken);

        return sourceRevenu.ToDto();
    }
}
