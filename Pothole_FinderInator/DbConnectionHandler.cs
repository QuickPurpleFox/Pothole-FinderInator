using System;
using System.Threading.Tasks;
using Application = Xamarin.Forms.Application;
using Npgsql;
using NpgsqlTypes;

namespace Pothole_FinderInator
{
    public static class DbConnectionHandler
    {
        private static NpgsqlConnectionStringBuilder _connString;
        private static NpgsqlConnection _conn;
        
        public static string UserName = null;

            
        static DbConnectionHandler()
        {
            Application.Current.Resources.TryGetValue("DATABASE_URL", out var dbUrl);
            
            Uri databaseUrl = new Uri(dbUrl.ToString());
            
            _connString = new NpgsqlConnectionStringBuilder();
            _connString.Host = "pothole-cockroach-8522.7tc.aws-eu-central-1.cockroachlabs.cloud";
            _connString.Port = 26257;
            
            var items = databaseUrl.UserInfo.Split(new[] { ':' });
            _connString.Username = "28860";
            _connString.Password = "g9KICM4DEzViwadNJ-8S_w";
            
            _connString.Database = "pothole_finder";
            _connString.SslMode = SslMode.Require;

            _ = DbConnectionStart();
        }

        private static async Task DbConnectionStart()
        {
            try
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connString.ConnectionString);
                var dataSource = dataSourceBuilder.Build();

                _conn = await dataSource.OpenConnectionAsync();
                await _conn.OpenAsync();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e);
                throw new NpgsqlException();
            }
        }
        
        private static int UserExistsCheck(string username, string password)
        {
            var userCheck = 0;
            using (var cmd = new NpgsqlCommand())
            {
                try
                {
                    _conn = new NpgsqlConnection(_connString.ConnectionString);
                    _conn.Open();
                    cmd.Connection = _conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM users WHERE username=@username AND password=@password;";
                    cmd.Parameters.AddWithValue("username", NpgsqlDbType.Text, username);
                    cmd.Parameters.AddWithValue("password", NpgsqlDbType.Text, password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userCheck = reader.GetInt32(0);
                        }

                        return userCheck;
                    }
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e);
                    throw new NpgsqlException();
                }
                catch (System.InvalidOperationException e)
                {
                    Console.WriteLine();
                    return 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return -1;
                }
            }
        }

        public static int Login(string username, string password)
        {
            return UserExistsCheck(username, password);
        }
    }
}