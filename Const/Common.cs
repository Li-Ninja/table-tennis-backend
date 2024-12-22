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

public enum RacketTypeEnum
{
    Shakehand = 1,
    Penhold = 2,
    ChinesePenhold = 3
}

public enum RubberTypeEnum
{
    None = 0,
    Inverted = 1,
    ShortPimple = 2,
    MiddlePimple = 3,
    LongPimple = 4,
    Anti = 5
}

public enum PlayerStatusEnum
{
    Freeze = 0,
    Play = 1,
    OnLeave = 2
}

public enum PlayerComparisonTypeEnum
{
    All = 0,
    Recent = 1,
    Annual = 2
}
