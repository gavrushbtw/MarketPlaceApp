using System;
using System.Configuration;
using Npgsql;

public class DatabaseHelper
{
    private string GetConnectionString()
    {
        string mode = ConfigurationManager.AppSettings["AppMode"];
        if (mode == "Production")
            return ConfigurationManager.ConnectionStrings["MarketplaceDB_Prod"].ConnectionString;
        return ConfigurationManager.ConnectionStrings["MarketplaceDB_Dev"].ConnectionString;
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(GetConnectionString());
    }

    public bool TestConnection()
    {
        try
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            using (var connection = GetConnection())
            {
                connection.Open();
                sw.Stop();
                Logger.Log($"Время подключения к БД: {sw.ElapsedMilliseconds} мс");
                return true;
            }
        }
        catch (Exception ex)
        {
            Logger.Log($"ОШИБКА подключения к БД: {ex.Message}");
            return false;
        }
    }
}