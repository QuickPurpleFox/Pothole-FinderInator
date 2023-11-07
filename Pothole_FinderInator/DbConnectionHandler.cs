using Application = Xamarin.Forms.Application;
using Npgsql;

namespace Pothole_FinderInator
{
    public static class DbConnectionHandler
    {
        private static string _DbLogib;
        private static string _DbPassword;
        private static string _connString;
            
        static DbConnectionHandler()
        {
            _DbLogib = (string)Application.Current.Resources["DbLogin"];
            _DbPassword = (string)Application.Current.Resources["DbPassword"];
            _connString = "postgresql://" + _DbLogib + ":" + _DbPassword +
                          "@lithe-sage-11501.8nj.cockroachlabs.cloud:26257/holedetectorinator?sslmode=verify-full";

            DbConnectionStart();
        }

        static async void DbConnectionStart()
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();
        }

        public static string GetUserName()
        {
            return "test";
        }
    }
}