using Budget.Application.Dtos.TypesSourceRevenu;
using Budget.Domain.Entities;

namespace Budget.Application.Features.TypesSourceRevenu;

internal static class TypeSourceRevenuMappingExtensions
{
    public static TypeSourceRevenuDto ToDto(this TypeSourceRevenu typeSourceRevenu) => new(
        typeSourceRevenu.Id,
        typeSourceRevenu.Nom);
}
