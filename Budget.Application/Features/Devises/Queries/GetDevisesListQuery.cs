using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.Devises;
using Mediator;

namespace Budget.Application.Features.Devises.Queries;

public sealed record GetDevisesListQuery(int Page, int PageSize) : IQuery<PagedResultDto<DeviseDto>>;

public sealed class GetDevisesListQueryHandler(IDeviseRepository deviseRepository)
    : IQueryHandler<GetDevisesListQuery, PagedResultDto<DeviseDto>>
{
    public async ValueTask<PagedResultDto<DeviseDto>> Handle(GetDevisesListQuery query, CancellationToken cancellationToken)
    {
        var devises = await deviseRepository.GetAllAsync(query.Page, query.PageSize, cancellationToken);

        return new PagedResultDto<DeviseDto>(
            devises.Items.Select(d => d.ToDto()).ToList(),
            devises.TotalCount,
            devises.Page,
            devises.PageSize);
    }
}
