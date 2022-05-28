using System.ComponentModel;

namespace API.Req;

public class UserRegister
{

    /// <summary>
    /// FirstName
    /// </summary>
    [Description("FirstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// LastName
    /// </summary>
    [Description("LastName")]
    public string LastName { get; set; }

    /// <summary>
    /// UserName
    /// </summary>
    [Description("UserName")]
    public string UserName { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [Description("Email")]
    public string Email { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    [Description("Phone")]
    public string Phone { get; set; }

    /// <summary>
    ///Password
    /// </summary>
    [Description("Password")]
    public string Password { get; set; }
}