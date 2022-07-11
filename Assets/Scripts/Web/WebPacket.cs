public class CreateAccountReq
{
    public string Username;
    public string Password;
}

public class LoginAccountReq
{
    public string Username;
    public string Password;
}

public class CreateAccountRes
{
    public bool Success;
}

public class LoginAccountRes
{
    public bool Success;
    public string Token;
}