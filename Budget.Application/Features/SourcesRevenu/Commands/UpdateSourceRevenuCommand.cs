using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Commands;

public sealed record UpdateSourceRevenuCommand(Guid Id, string Nom) : ICommand<SourceRevenuDto>;

public sealed class UpdateSourceRevenuCommandHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<UpdateSourceRevenuCommand, SourceRevenuDto>
{
    public async ValueTask<SourceRevenuDto> Handle(UpdateSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var sourceRevenu = await sourceRevenuRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), command.Id);

        SourceRevenuAuthorizationGuard.EnsureOwnership(sourceRevenu, currentUserService);

        sourceRevenu.Renommer(command.Nom);

        await sourceRevenuRepository.UpdateAsync(sourceRevenu, cancellationToken);

        return sourceRevenu.ToDto();
    }
}
