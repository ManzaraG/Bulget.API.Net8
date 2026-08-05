using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Categories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Categories.Commands;

public sealed record UpdateCategorieCommand(Guid Id, string Nom) : ICommand<CategorieDto>;

public sealed class UpdateCategorieCommandHandler(ICategorieRepository categorieRepository)
    : ICommandHandler<UpdateCategorieCommand, CategorieDto>
{
    public async ValueTask<CategorieDto> Handle(UpdateCategorieCommand command, CancellationToken cancellationToken)
    {
        var categorie = await categorieRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Categorie), command.Id);

        categorie.Renommer(command.Nom);

        await categorieRepository.UpdateAsync(categorie, cancellationToken);

        return categorie.ToDto();
    }
}
