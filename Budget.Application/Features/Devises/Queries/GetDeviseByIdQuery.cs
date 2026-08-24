using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Devises;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Devises.Queries;

public sealed record GetDeviseByIdQuery(Guid Id) : IQuery<DeviseDto>;

public sealed class GetDeviseByIdQueryHandler(IDeviseRepository deviseRepository)
    : IQueryHandler<GetDeviseByIdQuery, DeviseDto>
{
    public async ValueTask<DeviseDto> Handle(GetDeviseByIdQuery query, CancellationToken cancellationToken)
    {
        var devise = await deviseRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Devise), query.Id);

        return devise.ToDto();
    }
}
