using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Commands;

public sealed record UpdateSourceRevenuCommand(Guid Id, string Nom, Guid TypeId, Guid DeviseId) : ICommand<SourceRevenuDto>;

public sealed class UpdateSourceRevenuCommandHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ITypeSourceRevenuRepository typeSourceRevenuRepository,
    IDeviseRepository deviseRepository,
    ICurrentUserService currentUserService) : ICommandHandler<UpdateSourceRevenuCommand, SourceRevenuDto>
{
    public async ValueTask<SourceRevenuDto> Handle(UpdateSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var sourceRevenu = await sourceRevenuRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), command.Id);

        SourceRevenuAuthorizationGuard.EnsureOwnership(sourceRevenu, currentUserService);

        _ = await typeSourceRevenuRepository.GetByIdAsync(command.TypeId, cancellationToken)
            ?? throw new NotFoundException(nameof(TypeSourceRevenu), command.TypeId);

        _ = await deviseRepository.GetByIdAsync(command.DeviseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Devise), command.DeviseId);

        sourceRevenu.Renommer(command.Nom);
        sourceRevenu.ChangerType(command.TypeId);
        sourceRevenu.ChangerDevise(command.DeviseId);

        await sourceRevenuRepository.UpdateAsync(sourceRevenu, cancellationToken);

        return sourceRevenu.ToDto();
    }
}
