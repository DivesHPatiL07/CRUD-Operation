using Microsoft.Data.SqlClient;

namespace CRUD_Operation.Models
{
    public class DbHandlerBase
    {
        public static SqlConnection GetConnection()
        {
            //IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            string str = ConnectionStrings.DB_Conn_String;
            SqlConnection con = new SqlConnection(str);
            return con;
        }
    }
}
