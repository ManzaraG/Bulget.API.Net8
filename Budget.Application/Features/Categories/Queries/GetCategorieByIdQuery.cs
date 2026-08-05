using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Categories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Categories.Queries;

public sealed record GetCategorieByIdQuery(Guid Id) : IQuery<CategorieDto>;

public sealed class GetCategorieByIdQueryHandler(ICategorieRepository categorieRepository)
    : IQueryHandler<GetCategorieByIdQuery, CategorieDto>
{
    public async ValueTask<CategorieDto> Handle(GetCategorieByIdQuery query, CancellationToken cancellationToken)
    {
        var categorie = await categorieRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Categorie), query.Id);

        return categorie.ToDto();
    }
}
