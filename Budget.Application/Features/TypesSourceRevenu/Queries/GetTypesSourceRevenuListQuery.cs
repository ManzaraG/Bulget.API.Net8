using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.TypesSourceRevenu;
using Mediator;

namespace Budget.Application.Features.TypesSourceRevenu.Queries;

public sealed record GetTypesSourceRevenuListQuery(int Page, int PageSize) : IQuery<PagedResultDto<TypeSourceRevenuDto>>;

public sealed class GetTypesSourceRevenuListQueryHandler(ITypeSourceRevenuRepository typeSourceRevenuRepository)
    : IQueryHandler<GetTypesSourceRevenuListQuery, PagedResultDto<TypeSourceRevenuDto>>
{
    public async ValueTask<PagedResultDto<TypeSourceRevenuDto>> Handle(GetTypesSourceRevenuListQuery query, CancellationToken cancellationToken)
    {
        var typesSourceRevenu = await typeSourceRevenuRepository.GetAllAsync(query.Page, query.PageSize, cancellationToken);

        return new PagedResultDto<TypeSourceRevenuDto>(
            typesSourceRevenu.Items.Select(t => t.ToDto()).ToList(),
            typesSourceRevenu.TotalCount,
            typesSourceRevenu.Page,
            typesSourceRevenu.PageSize);
    }
}
