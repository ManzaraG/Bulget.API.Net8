using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Categories;
using Budget.Application.Dtos.Common;
using Mediator;

namespace Budget.Application.Features.Categories.Queries;

public sealed record GetCategoriesListQuery(int Page, int PageSize) : IQuery<PagedResultDto<CategorieDto>>;

public sealed class GetCategoriesListQueryHandler(ICategorieRepository categorieRepository)
    : IQueryHandler<GetCategoriesListQuery, PagedResultDto<CategorieDto>>
{
    public async ValueTask<PagedResultDto<CategorieDto>> Handle(GetCategoriesListQuery query, CancellationToken cancellationToken)
    {
        var categories = await categorieRepository.GetAllAsync(query.Page, query.PageSize, cancellationToken);

        return new PagedResultDto<CategorieDto>(
            categories.Items.Select(c => c.ToDto()).ToList(),
            categories.TotalCount,
            categories.Page,
            categories.PageSize);
    }
}
