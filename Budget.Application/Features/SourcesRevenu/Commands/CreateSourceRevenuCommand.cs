using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Commands;

public sealed record CreateSourceRevenuCommand(string Nom, Guid TypeId, Guid DeviseId) : ICommand<SourceRevenuDto>;

public sealed class CreateSourceRevenuCommandHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ITypeSourceRevenuRepository typeSourceRevenuRepository,
    IDeviseRepository deviseRepository,
    ICurrentUserService currentUserService) : ICommandHandler<CreateSourceRevenuCommand, SourceRevenuDto>
{
    public async ValueTask<SourceRevenuDto> Handle(CreateSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        _ = await typeSourceRevenuRepository.GetByIdAsync(command.TypeId, cancellationToken)
            ?? throw new NotFoundException(nameof(TypeSourceRevenu), command.TypeId);

        _ = await deviseRepository.GetByIdAsync(command.DeviseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Devise), command.DeviseId);

        var sourceRevenu = new SourceRevenu(command.Nom, command.TypeId, command.DeviseId, utilisateurId);

        await sourceRevenuRepository.AddAsync(sourceRevenu, cancellationToken);

        return sourceRevenu.ToDto();
    }
}
