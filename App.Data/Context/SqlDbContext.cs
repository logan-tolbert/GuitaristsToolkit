using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace App.Data.Context;

public class SqlDbContext : ISqlDbContext
{
    private readonly IConfiguration _config;

    public SqlDbContext(IConfiguration config)
    {
        _config = config;
    }

    public IEnumerable<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionName = "Default", bool isStoredProcedure = false)
    {
        string connectionString = _config.GetConnectionString(connectionName)!;
        CommandType commandType = CommandType.Text;

        if (isStoredProcedure == true)
        {
            commandType = CommandType.StoredProcedure;
        }

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            IEnumerable<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
            return rows;
        }
    }

    public void SaveData<T, U>(string sqlStatement, U parameters, string connectionName = "Default", bool isStoredProcedure = false)
    {
        string connectionString = _config.GetConnectionString(connectionName)!;

        CommandType commandType = CommandType.Text;

        if (isStoredProcedure == true)
        {
            commandType = CommandType.StoredProcedure;
        }

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Execute(sqlStatement, parameters, commandType: commandType);
        }
    }
}
