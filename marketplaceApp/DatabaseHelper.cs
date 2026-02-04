using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

public class DatabaseHelper
{
    private string connectionString = @"Data Source=FAFLA666\SQLEXPRESS;Initial Catalog=MarketplaceDB;Integrated Security=True;";

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }

    // Проверка подключения
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