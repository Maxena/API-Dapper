namespace API.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName) : base($"User: {userName} Was Not Found :(  ")
    {
    }
}