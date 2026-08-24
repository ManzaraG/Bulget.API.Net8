using Budget.Application.Dtos.Devises;
using Budget.Domain.Entities;

namespace Budget.Application.Features.Devises;

internal static class DeviseMappingExtensions
{
    public static DeviseDto ToDto(this Devise devise) => new(
        devise.Id,
        devise.Nom);
}
