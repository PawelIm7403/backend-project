namespace CoreApp.Authorization;

public enum AppPolicies
{
    AdminOnly,
    RegisteredUserOnly,
    AnonymousUserOnly
}

public static class AppPoliciesExtensions
{
    public static string Name(this AppPolicies policy)
    {
        return policy.ToString();
    }
}