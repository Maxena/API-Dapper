using API.Interfaces;
using API.Req;
using API.Res;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/{v1:apiVersion}/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository _repo;

    public UserController(IRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// List of all users 
    /// </summary>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("")]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        try
        {
            var users = await _repo.GetUsers();
            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// specific user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUserById(int id)
    {
        try
        {
            var user = await _repo.GetUserById(id);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// user login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponse>> Login(UserLogin request)
    {
        try
        {
            var user = await _repo.Login(request);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Register a new user 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<ActionResult<string>> Register([FromForm] UserRegister request)
    {
        try
        {
            var user = await _repo.Register(request);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}