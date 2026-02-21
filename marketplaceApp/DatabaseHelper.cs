using System;
using System.Data.SqlClient;
using System.Configuration;

public class DatabaseHelper
{
    private string GetConnectionString()
    {
        string mode = ConfigurationManager.AppSettings["AppMode"];

        if (mode == "Production")
            return ConfigurationManager.ConnectionStrings["MarketplaceDB_Prod"].ConnectionString;

        return ConfigurationManager.ConnectionStrings["MarketplaceDB_Dev"].ConnectionString;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(GetConnectionString());
    }

    public bool TestConnection()
    {
        try
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}