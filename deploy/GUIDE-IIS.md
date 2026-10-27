# Déploiement Budget.API sur IIS (Windows Server + SQL Server)

Fichiers livrés : `publish/` (l'application) et `migrations.sql` (schéma de la base).

## 1. Prérequis sur le serveur (à demander à l'IT du client)
- Windows Server avec le rôle **IIS** (+ fonctionnalité *WebSocket* facultative).
- **.NET 10 Hosting Bundle** (https://dotnet.microsoft.com/download/dotnet/10.0, "Hosting Bundle"), puis `iisreset`.
- **SQL Server** accessible depuis le serveur, avec le droit de créer une base.
- Un certificat HTTPS (ou un reverse proxy qui s'en charge).

## 2. Base de données
1. Créer une base vide `BudgetDb`.
2. Exécuter `migrations.sql` dessus (SSMS ou `sqlcmd -S <serveur> -d BudgetDb -i migrations.sql`). Le script est idempotent : on peut le rejouer à chaque mise à jour.
3. Créer un compte SQL (ou donner le droit à l'identité du pool IIS) avec `db_datareader`, `db_datawriter`, `db_ddladmin`
   (db_ddladmin : le schéma "apikeys" est créé au démarrage de l'API).

## 3. Site IIS
1. Copier le contenu de `publish/` dans `C:\inetpub\budget-api\`.
2. IIS > Pools d'applications > nouveau pool : **Aucun code managé**, pipeline intégré.
3. IIS > Sites > Ajouter un site : chemin `C:\inetpub\budget-api`, liaison HTTPS + certificat, pool ci-dessus.
4. Donner le droit *Lecture/Exécution* sur le dossier à `IIS AppPool\<nom du pool>`.

## 4. Configuration (secrets)
Ne rien mettre dans les fichiers. Définir les variables d'environnement dans `web.config`
(`<aspNetCore> > <environmentVariables>`) ou au niveau du pool :

| Variable | Valeur |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__BudgetDatabase` | `Server=...;Database=BudgetDb;User Id=...;Password=...;TrustServerCertificate=True;` |
| `ApiKeysDatabase__ConnectionString` | la même chaîne |
| `ApiKeysDatabase__Provider` | `sqlserver` |
| `Jwt__SecretKey` | une clé aléatoire d'au moins 32 caractères (à générer, différente de celle du dev) |
| `Cors__AllowedOrigins__0` | URL du frontend, ex. `https://budget.client.com` |

Exemple dans web.config :
```xml
<aspNetCore processPath="dotnet" arguments=".\Budget.API.dll" hostingModel="inprocess">
  <environmentVariables>
    <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
    <environmentVariable name="Jwt__SecretKey" value="..." />
  </environmentVariables>
</aspNetCore>
```
Si le fichier web.config est écrasé à un redéploiement, garder une copie.

## 5. Vérifier
Redémarrer le site, puis appeler une route de l'API en HTTPS. En cas d'erreur 500.30 : activer `stdoutLogEnabled="true"`
dans web.config (dossier `logs\` à créer) ou consulter l'Observateur d'événements.
Swagger est volontairement désactivé hors Development.

## 6. Mise à jour
Générer un nouveau `publish/` + `migrations.sql`, mettre `app_offline.htm` dans le dossier du site, copier les fichiers
(sans écraser web.config), rejouer le script SQL, supprimer `app_offline.htm`.

## Régénérer les livrables (poste de dev)
```
dotnet publish Budget.API -c Release -o deploy/publish
dotnet ef migrations script --idempotent -c BudgetDbContext -p Budget.Infrastructure.Persistence -o deploy/migrations.sql
```
