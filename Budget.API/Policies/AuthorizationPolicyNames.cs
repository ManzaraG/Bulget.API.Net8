namespace Budget.API.Policies;

public static class AuthorizationPolicyNames
{
    public const string RequireAuthenticatedUser = "RequireAuthenticatedUser";

    /// <summary>Accès par clé API (scheme ApiKey) portant le scope Transactions:Read.</summary>
    public const string TransactionsReadPolicy = "TransactionsReadPolicy";

    /// <summary>Accès par clé API (scheme ApiKey) portant le scope Dashboard:Read.</summary>
    public const string DashboardReadPolicy = "DashboardReadPolicy";

    /// <summary>Accès par clé API (scheme ApiKey) portant le scope Categories:Read.</summary>
    public const string CategoriesReadPolicy = "CategoriesReadPolicy";
}
