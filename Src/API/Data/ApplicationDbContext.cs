using System.Data;
using Microsoft.Data.SqlClient;

namespace API.Data;

public class ApplicationDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlServer");
    }

    public IDbConnection CreateConnection
        => new SqlConnection(_connectionString);
}