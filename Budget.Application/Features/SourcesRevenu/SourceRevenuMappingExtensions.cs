using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;

namespace Budget.Application.Features.SourcesRevenu;

internal static class SourceRevenuMappingExtensions
{
    public static SourceRevenuDto ToDto(this SourceRevenu sourceRevenu) => new(
        sourceRevenu.Id,
        sourceRevenu.Nom,
        sourceRevenu.TypeId,
        sourceRevenu.EstActif,
        sourceRevenu.UtilisateurId,
        sourceRevenu.DateCreation);
}
