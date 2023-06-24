
using Microsoft.VisualBasic.ApplicationServices;
using Npgsql;
using System.Xml.Linq;

namespace Api_Apathe
{
    public class Connection_Database_Postgres
    {
        
        public string connectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=1";

        public static string conectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=1";

        public void Create_Table_User(Connection_Database_Postgres _Postgres)
        {
            using (var connection = new NpgsqlConnection(_Postgres.connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;

                    // Создание таблицы с Пользователями
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Users \r\n(  Id_Users  Serial not null CONSTRAINT   PK_Id_Users Primary key," +
                        " \r\n Name TEXT  NOT NULL  ," +
                        " \r\nPassword TEXT NOT NULL\r\n);";

                    command.ExecuteNonQuery();

                    // Создание таблицы с логами
                    command.CommandText = "CREATE  TABLE   IF NOT EXISTS  logs (\r\n\r\n\t" +
                                          "Id_logs Serial not null CONSTRAINT   " +
                                          "PK_Id_logs Primary key ,\r\n\t " +
                                          "ip Varchar not null, \r\n\t" +
                                          "date TIMESTAMP, \r\n\t" +
                                          "request Varchar,\r\n\t" +
                                          "status Varchar not null,\r\n\t" +
                                          "size INTEGER not null\r\n);";
                    command.ExecuteNonQuery();
                }
            }
        }


        public bool Check_user(bool Users, Connection_Database_Postgres _Postgres, string Name, string Password)
        {

            using (var connection = new NpgsqlConnection(_Postgres.connectionString))
            {
                connection.Open();

                var Check_users = $" Select id_users from users where users.Name  = '{Name}' and users.Password = '{Password}'";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(Check_users, connection);

                NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(Check_users, connection);

                npgsqlCommand.ExecuteNonQuery();

                NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                if (npgsqlDataReader.HasRows == true)
                {
                    Users = true;
                }
                else
                {
                    Users = false;
                }
            }
            return Users;
        }

        public bool Insert_user(bool Users, Connection_Database_Postgres _Postgres, string Name, string Password)
        {

            string Sql_Select = $"INSERT INTO Users(Name, Password) VALUES ('{Name}', '{Password}');";
            using (var connection = new NpgsqlConnection(_Postgres.connectionString))
            {
                connection.Open();

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(Sql_Select, connection);
                npgsqlCommand.ExecuteNonQuery();
                Users = true;
            }
            return Users;

        }
    }
}
