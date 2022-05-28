using API.Req;
using API.Res;

namespace API.Interfaces;

public interface IRepository
{
    Task<string> Register(UserRegister request);
    Task<LoginResponse> Login(UserLogin request);
    Task<UserResponse> GetUserById(int id);
    Task<List<UserResponse>> GetUsers();
}