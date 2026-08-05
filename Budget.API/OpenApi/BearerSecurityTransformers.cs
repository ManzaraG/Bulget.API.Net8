using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Budget.API.OpenApi;

/// <summary>
/// Déclare le schéma de sécurité "Bearer" dans le document OpenAPI, ce qui fait apparaître
/// le bouton "Authorize" dans Swagger UI.
/// </summary>
internal sealed class BearerSecuritySchemeDocumentTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var schemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (!schemes.Any(scheme => scheme.Name == JwtBearerDefaults.AuthenticationScheme))
            return;

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            [JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Token JWT obtenu via /api/auth/login ou /api/auth/register.",
            },
        };
    }
}

/// <summary>
/// Ajoute le cadenas sur chaque endpoint qui exige une authentification, en l'omettant pour
/// ceux marqués [AllowAnonymous] (ex. login/register).
/// </summary>
internal sealed class BearerSecurityRequirementOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var hasAllowAnonymous = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>()
            .Any();

        if (hasAllowAnonymous)
            return Task.CompletedTask;

        operation.Security ??= [];
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, context.Document)] = [],
        });

        return Task.CompletedTask;
    }
}
