using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Categories.Commands;

public sealed record DeleteCategorieCommand(Guid Id) : ICommand;

public sealed class DeleteCategorieCommandHandler(ICategorieRepository categorieRepository)
    : ICommandHandler<DeleteCategorieCommand>
{
    public async ValueTask<Unit> Handle(DeleteCategorieCommand command, CancellationToken cancellationToken)
    {
        _ = await categorieRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Categorie), command.Id);

        await categorieRepository.DeleteAsync(command.Id, cancellationToken);

        return Unit.Value;
    }
}
