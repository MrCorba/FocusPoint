public class UserService : IUserService
{
    public string GetLocalUserId(HttpContext context)
    {
        // In your app, replace with real logged-in user id (from cookie/session/claims).
        return "demo-user-1";
    }
}