namespace App.Data.Context;

public interface ISqlDbContext
{
    IEnumerable<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionName = "Default", bool isStoredProcedure = false);
    void SaveData<T, U>(string sqlStatement, U parameters, string connectionName = "Default", bool isStoredProcedure = false);
}