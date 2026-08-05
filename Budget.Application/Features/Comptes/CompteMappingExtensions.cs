using Budget.Application.Dtos.Comptes;
using Budget.Domain.Entities;

namespace Budget.Application.Features.Comptes;

internal static class CompteMappingExtensions
{
    public static CompteDto ToDto(this Compte compte) => new(
        compte.Id,
        compte.Nom,
        compte.UtilisateurId,
        compte.DateCreation);
}
