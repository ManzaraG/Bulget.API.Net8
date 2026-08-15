using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Commands;

public sealed record ActivateSourceRevenuCommand(Guid Id) : ICommand<SourceRevenuDto>;

public sealed class ActivateSourceRevenuCommandHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<ActivateSourceRevenuCommand, SourceRevenuDto>
{
    public async ValueTask<SourceRevenuDto> Handle(ActivateSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var sourceRevenu = await sourceRevenuRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), command.Id);

        SourceRevenuAuthorizationGuard.EnsureOwnership(sourceRevenu, currentUserService);

        sourceRevenu.Activer();

        await sourceRevenuRepository.UpdateAsync(sourceRevenu, cancellationToken);

        return sourceRevenu.ToDto();
    }
}
