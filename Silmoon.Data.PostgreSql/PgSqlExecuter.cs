using Npgsql;

namespace Silmoon.Data.PostgreSql
{
    public class PgSqlExecuter
    {
        NpgsqlConnection Connection { get; }
        public PgSqlExecuter(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }
    }
}
