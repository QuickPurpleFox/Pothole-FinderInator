using System;
using System.Threading.Tasks;
using Application = Xamarin.Forms.Application;
using Npgsql;

namespace Pothole_FinderInator
{
    public static class DbConnectionHandler
    {
        private static string _connString;

        public static string UserName = "Admin";

            
        static DbConnectionHandler()
        {
            
            Application.Current.Resources.TryGetValue("DbLogin", out var dbLogin);
            Application.Current.Resources.TryGetValue("DbPassword", out var dbPassword);
            //_connString = "postgresql://" + dbLogin + ":" + dbPassword +
            //              "@lithe-sage-11501.8nj.cockroachlabs.cloud:26257/holedetectorinator?sslmode=verify-full";
            _connString = "postgresql://haniasmolej:T0DpVj7xcYmeJaaBSCYeQw@lithe-sage-11501.8nj.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full";

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

        public static void ExecuteCommand(string command)
        {
            var conn = new NpgsqlConnection(_connString);
            var cmd = new NpgsqlCommand(command, conn);
            cmd.ExecuteNonQuery();
        }
        
        public static int UserExistsCheck(string username, string password)
        {
            var conn = new NpgsqlConnection(_connString);
            var cmd = new NpgsqlCommand(string.Format("SELECT COUNT(*) FROM users WHERE login={0} AND password={1};",username, password), conn);
            int userCheck = cmd.ExecuteReader().GetInt32(0);
            return userCheck;
        }
        public static int HoleExistsCheck(float coord_ns, float coord_we)
        {
            var conn = new NpgsqlConnection(_connString);
            var cmd = new NpgsqlCommand(string.Format("SELECT COUNT(*) FROM holes WHERE coord_ns={0} AND coord_we={1};", coord_ns, coord_we), conn);
            int userCheck = cmd.ExecuteReader().GetInt32(0);
            return userCheck;
        }

        public static int UserNumberCheck()
        {
            var conn = new NpgsqlConnection(_connString);
            var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users;", conn);
            int userNumberCheck = cmd.ExecuteReader().GetInt32(0);
            return userNumberCheck;
        }
        public static int HoleNumberCheck()
        {
            var conn = new NpgsqlConnection(_connString);
            var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM holes;", conn);
            int userNumberCheck = cmd.ExecuteReader().GetInt32(0);
            return userNumberCheck;
        }


        public static bool Login(string username, string password)
        {
            if (UserExistsCheck(username, password) == 1)
                return true;
            else
                return false;
        }

        public static string register(string inputlogin, string inputpassword)
        {
            int newuser_id = UserNumberCheck() + 1;
            if (UserExistsCheck(inputlogin, inputpassword) == 0)
            {
                ExecuteCommand(string.Format("INSERT INTO users VALUES ({0}, '{1}', '{2}');", newuser_id, inputlogin, inputpassword));
                return "You've registered succesfully";
            }
               
            else
                return "UserAlreadyExists";
        }
        public static string AddPothole(float coord_ns, float coord_we, string holeSize)
        {
            int newhole_id = HoleNumberCheck() + 1;
            if (HoleExistsCheck(coord_ns, coord_we) == 0)
            {
                ExecuteCommand(string.Format("INSERT INTO users VALUES ({0}, {1}, {2}, {3});", newhole_id, coord_ns, coord_we, holeSize));
                return "Pothole succesfully added";
            }
            else
                return "Pothole was already added, would you like to change something?";
        }
        public static string UpdatePotholeSize(int hole_id, string holesize)
        {
            ExecuteCommand(string.Format("UPDATE holes SET hole_size={0} WHERE id={1}",holesize,hole_id));
            return "Pothole size updated";
        }
        public static string DeletePothole(int hole_id, float coord_ns, float coord_we)
        {
            ExecuteCommand(string.Format("DELETE FROM holes WHERE id = {0} OR (coord_ns = {1} AND coord_we = {2});", hole_id, coord_ns, coord_we));
            return "Hole deleted";
        }
    }
}