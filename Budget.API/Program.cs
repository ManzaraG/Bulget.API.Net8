using Budget.API.AuthenticationServices;
using Budget.API.Middlewares;
using Budget.API.OpenApi;
using Budget.API.Policies;
using Budget.Application.Adapters.Identities;
using Budget.Application.Adapters.Passwords;
using Budget.Application.Adapters.Tokens;
using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Services;
using Budget.Infrastructure.Persistence;
using Inexa.ApiKeys.Core;
using Inexa.ApiKeys.Dapper.SqlServer;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

const string FrontendCorsPolicy = "FrontendDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:5174"];
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeDocumentTransformer>();
    options.AddOperationTransformer<BearerSecurityRequirementOperationTransformer>();
});

builder.Services.AddMediator(options =>
{
    // Les handlers dépendent de repositories adossés à un DbContext scoped :
    // le lifetime par défaut (Singleton) provoquerait une dépendance captive.
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

var connectionString = builder.Configuration.GetConnectionString("BudgetDatabase")
    ?? throw new InvalidOperationException("La chaîne de connexion 'BudgetDatabase' est manquante.");
builder.Services.AddPersistence(connectionString);

var apiKeysConnectionString = builder.Configuration["ApiKeysDatabase:ConnectionString"]!;

// Fabrique de connexion : une nouvelle connexion par appel, conforme aux bonnes pratiques
// Dapper/ADO.NET (pas de connexion partagée entre requêtes concurrentes).
Func<IDbConnection> apiKeysConnectionFactory = () => new SqlConnection(apiKeysConnectionString);

builder.Services.AddSingleton(apiKeysConnectionFactory);
builder.Services.AddSingleton<IApiKeyRepository>(sp =>
    new SqlServerApiKeyRepository(sp.GetRequiredService<Func<IDbConnection>>()));
builder.Services.AddScoped<IApiKeyIssuer, ApiKeyIssuer>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.SchemeName, options => { });
builder.Services.AddApiAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Applique le schéma "apikeys" au démarrage (idempotent). La base BudgetDb existe déjà
// (utilisée par EF Core), donc pas de logique de création de base nécessaire ici.
var apiKeysRunner = new SchemaMigrationRunner(apiKeysConnectionFactory);
await apiKeysRunner.RunAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Budget API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
