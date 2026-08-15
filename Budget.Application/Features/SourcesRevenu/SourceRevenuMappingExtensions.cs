using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.DomainEnums;
using Budget.Domain.Entities;

namespace Budget.Application.Features.SourcesRevenu;

internal static class SourceRevenuMappingExtensions
{
    public static SourceRevenuDto ToDto(this SourceRevenu sourceRevenu) => new(
        sourceRevenu.Id,
        sourceRevenu.Nom,
        sourceRevenu.Type.ToDto(),
        sourceRevenu.EstActif,
        sourceRevenu.UtilisateurId,
        sourceRevenu.DateCreation);

    public static TypeSourceRevenuDto ToDto(this TypeSourceRevenu type) => type switch
    {
        TypeSourceRevenu.Salaire => TypeSourceRevenuDto.Salaire,
        TypeSourceRevenu.Freelance => TypeSourceRevenuDto.Freelance,
        TypeSourceRevenu.Investissement => TypeSourceRevenuDto.Investissement,
        TypeSourceRevenu.Location => TypeSourceRevenuDto.Location,
        _ => TypeSourceRevenuDto.Autre,
    };

    public static TypeSourceRevenu ToDomain(this TypeSourceRevenuDto type) => type switch
    {
        TypeSourceRevenuDto.Salaire => TypeSourceRevenu.Salaire,
        TypeSourceRevenuDto.Freelance => TypeSourceRevenu.Freelance,
        TypeSourceRevenuDto.Investissement => TypeSourceRevenu.Investissement,
        TypeSourceRevenuDto.Location => TypeSourceRevenu.Location,
        _ => TypeSourceRevenu.Autre,
    };
}
