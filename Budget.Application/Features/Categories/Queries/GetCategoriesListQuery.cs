using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Categories;
using Mediator;

namespace Budget.Application.Features.Categories.Queries;

public sealed record GetCategoriesListQuery : IQuery<IReadOnlyList<CategorieDto>>;

public sealed class GetCategoriesListQueryHandler(ICategorieRepository categorieRepository)
    : IQueryHandler<GetCategoriesListQuery, IReadOnlyList<CategorieDto>>
{
    public async ValueTask<IReadOnlyList<CategorieDto>> Handle(GetCategoriesListQuery query, CancellationToken cancellationToken)
    {
        var categories = await categorieRepository.GetAllAsync(cancellationToken);

        return categories.Select(c => c.ToDto()).ToList();
    }
}
