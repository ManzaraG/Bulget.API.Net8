using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Categories;
using Budget.Application.Dtos.Transactions;
using Budget.Domain.DomainEnums;
using Budget.Domain.Entities;
using Mediator;

namespace Budget.Application.Features.Categories.Commands;

public sealed record CreateCategorieCommand(string Nom, TypeTransactionDto Type) : ICommand<CategorieDto>;

public sealed class CreateCategorieCommandHandler(ICategorieRepository categorieRepository)
    : ICommandHandler<CreateCategorieCommand, CategorieDto>
{
    public async ValueTask<CategorieDto> Handle(CreateCategorieCommand command, CancellationToken cancellationToken)
    {
        var type = command.Type == TypeTransactionDto.Revenu ? TypeTransaction.Revenu : TypeTransaction.Depense;
        var categorie = new Categorie(command.Nom, type);

        await categorieRepository.AddAsync(categorie, cancellationToken);

        return categorie.ToDto();
    }
}
