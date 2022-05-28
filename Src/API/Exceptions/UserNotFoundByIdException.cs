namespace API.Exceptions;

public class UserNotFoundByIdException : Exception
{
    public UserNotFoundByIdException(int id) : base($"User with id {id} not found :( ")
    {
    }
}