using System.ComponentModel;

namespace API.Res;

public class LoginResponse
{
    /// <summary>
    /// UserId
    /// </summary>
    [Description("UserId")]
    public int UserId { get; set; }
    
    /// <summary>
    /// FullName
    /// </summary>
    [Description("FullName")]
    public string FullName { get; set; }
    
    /// <summary>
    /// Token
    /// </summary>
    [Description("Token")]
    public string Token { get; set; }
}