namespace table_tennis_backend.Const;

public class Auth
{
    public const string ValidToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkZha2VVc2VyIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE2MjYyMzkwMjJ9.mF_sZUO3W5eKpKfOqhPDwzG5M5eNqR6Sb4wZaM0Y6Mg;";

    public static bool ValidateToken(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            string token = authorizationHeader.ToString().Split(' ')[1];

            return token == ValidToken;
        }

        return false;
    }
}