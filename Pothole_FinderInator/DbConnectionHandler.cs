using System;
using System.Threading.Tasks;
using Application = Xamarin.Forms.Application;
using Npgsql;

namespace Pothole_FinderInator
{
    public static class DbConnectionHandler
    {
        private static string _connString;

        public static string UserName = null;
            
        static DbConnectionHandler()
        {
            
            Application.Current.Resources.TryGetValue("DbLogin", out var dbLogin);
            Application.Current.Resources.TryGetValue("DbPassword", out var dbPassword);
            _connString = "postgresql://" + dbLogin + ":" + dbPassword +
                          "@lithe-sage-11501.8nj.cockroachlabs.cloud:26257/holedetectorinator?sslmode=verify-full";

            _ = DbConnectionStart();
        }

        private static async Task DbConnectionStart()
        {
            try
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connString);
                var dataSource = dataSourceBuilder.Build();

                var conn = await dataSource.OpenConnectionAsync();
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }
        }

        public static string Login()
        {
            return "test";
        }

        public static int register()
        {
            return -1;
        }
    }
}