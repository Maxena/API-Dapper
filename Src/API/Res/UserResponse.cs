using System.ComponentModel;

namespace API.Res;

public class UserResponse
{
    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public string FirstName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public string LastName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public string UserName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public string Phone { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public DateTime LastLogin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Description("")]
    public DateTime CreationTime { get; set; }
}