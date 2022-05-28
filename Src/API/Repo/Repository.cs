using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using API.Data;
using API.Entities;
using API.Exceptions;
using API.Interfaces;
using API.Req;
using API.Res;
using Dapper;
using Microsoft.IdentityModel.Tokens;

namespace API.Repo;

public class Repository : IRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public Repository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> Register(UserRegister request)
    {
        const string query =
            "INSERT INTO Users (Email, Password, FirstName, LastName, Phone, LasLogin, CreationTime) " +
            "VALUES (@Email, @Password, @FirstName, @LastName, @Phone, @LasLogin, @CreationTime)";

        var parameters = new DynamicParameters();

        parameters.Add("@Email", request.Email);
        parameters.Add("@Password", request.Password);
        parameters.Add("@FirstName", request.FirstName);
        parameters.Add("@LastName", request.LastName);
        parameters.Add("@Phone", request.Phone);
        parameters.Add("@LasLogin", null);
        parameters.Add("@CreationTime", DateTime.Now);

        var connection = _context.CreateConnection;

        await connection.ExecuteAsync(query, parameters);

        return "User created!!";
    }

    public async Task<LoginResponse> Login(UserLogin request)
    {
        const string query = "Select * from Users where UserName = @UserName and Password = @Password";

        var parameters = new DynamicParameters();

        parameters.Add("@UserName", request.UserName);
        parameters.Add("@Password", request.Password);

        var connection = _context.CreateConnection;

        var user = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

        if (user is null)
            throw new UserNotFoundException(request.UserName);

        //todo: Generate token
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSetting:Key"]));

        var credential = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["JwtSetting:Issuer"],
            _configuration["JwtSetting:Audience"],
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credential);

        var res = new LoginResponse
        {
            UserId = user.Id,
            FullName = user.FirstName + " " + user.LastName,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };

        return res;
    }

    public async Task<UserResponse> GetUserById(int id)
    {
        const string query = "Select * from Users where Id = @Id";

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        var connection = _context.CreateConnection;

        var user = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

        if (user is null)
            throw new UserNotFoundByIdException(id);

        var res = new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            CreationTime = user.CreationTime,
            LastLogin = user.LastLogin,
            UserName = user.UserName
        };

        return res;
    }

    public async Task<List<UserResponse>> GetUsers()
    {
        const string query = "Select * from Users";

        var connection = _context.CreateConnection;

        var users = await connection.QueryAsync<User>(query);

        var res = new List<UserResponse>();

        res = users.Aggregate(res, (current, item) => current.Append(new UserResponse
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Phone = item.Phone,
                CreationTime = item.CreationTime,
                LastLogin = item.LastLogin,
                UserName = item.UserName
            })
            .ToList());

        return res;
    }
}