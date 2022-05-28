namespace API.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime CreationTime { get; set; }
}