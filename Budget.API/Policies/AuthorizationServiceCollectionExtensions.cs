using Microsoft.AspNetCore.Authorization;

namespace Budget.API.Policies;

/// <summary>
/// Politiques d'accès aux endpoints (« l'utilisateur est-il connecté ? »).
/// La règle « ne peut consulter/modifier que ses propres sources de revenu et transactions » est une
/// décision métier qui dépend de la ressource chargée : elle est appliquée dans
/// Budget.Application.Features.SourcesRevenu.SourceRevenuAuthorizationGuard, pas ici.
/// </summary>
public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build())
            .AddPolicy(AuthorizationPolicyNames.RequireAuthenticatedUser, policy => policy.RequireAuthenticatedUser());

        return services;
    }
}
