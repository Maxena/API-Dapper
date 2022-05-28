using System.ComponentModel;

namespace API.Req;

public class UserLogin
{
    /// <summary>
    /// UserName
    /// </summary>
    [Description("UserName")]
    public string UserName { get; set; }
    
    /// <summary>
    /// Password
    /// </summary>
    [Description("Password")]
    public string Password { get; set; }
}