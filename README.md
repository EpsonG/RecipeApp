# RecipeApp_Eboaguillaume

Application Blazor Server (.NET 9) de gestion de recettes, avec persistance MySQL
(accès aux données via Dapper) et génération assistée par IA (OpenAI) pour les
détails de recette.

## Structure du dépôt

- `RecipeApp_Eboaguillaume/` — application Blazor Server (UI, pages, services)
- `SQLAccess/` — bibliothèque d'accès aux données MySQL (Dapper)

## Prérequis

- .NET 9 SDK
- Un serveur MySQL accessible (local ou distant)
- (Optionnel) Une clé API OpenAI, si tu utilises la génération de recette par IA

## Configuration des secrets (en local)

Les fichiers `appsettings.json` et `appsettings.Development.json` de ce dépôt ne
contiennent **aucun secret** — les valeurs sensibles doivent être fournies via les
**Secrets utilisateur (.NET User Secrets)**, qui restent sur ta machine et ne sont
jamais commit.

Depuis le dossier `RecipeApp_Eboaguillaume/` :

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:MySqlConnection" "Server=127.0.0.1;Port=3306;database=blazor_recipes;user id=root;password=TON_MOT_DE_PASSE"
dotnet user-secrets set "OpenAI:ApiKey" "TA_CLE_OPENAI"
dotnet user-secrets set "Admin:Email" "admin@example.com"
dotnet user-secrets set "Admin:Password" "UnMotDePasseFort!"
```

`Admin:Email` / `Admin:Password` définissent le compte administrateur créé automatiquement
au démarrage (via ASP.NET Core Identity) s'il n'existe pas déjà. Ce compte est le seul
autorisé à accéder aux routes `/admin/*`. Au premier lancement, l'application applique
aussi automatiquement la migration EF Core qui crée les tables Identity (`AspNetUsers`,
`AspNetRoles`, etc.) dans la même base MySQL.

## Lancer le projet

```bash
dotnet restore
dotnet run --project RecipeApp_Eboaguillaume
```

## Déploiement (Azure App Service)

En production, ne mets jamais les secrets dans `appsettings.json`. Configure-les plutôt
dans **Azure Portal → App Service → Configuration → Application settings / Connection strings** :

- `ConnectionStrings__MySqlConnection` (ou `MySqlConnection` sous Connection strings, type MySQL)
- `OpenAI__ApiKey`
- `Admin__Email` / `Admin__Password`

## Notes de sécurité

Ce projet a été assaini avant sa mise sur GitHub : les identifiants MySQL et la clé
API OpenAI qui étaient en clair dans les fichiers de configuration ont été retirés.
Si ces identifiants ont déjà été utilisés ailleurs, il est recommandé de les régénérer.
