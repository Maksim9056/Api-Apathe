
//using Microsoft.VisualBasic.ApplicationServices;
//using Microsoft.VisualBasic.Logging;
using Npgsql;
using System.Globalization;
//using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
//using System.Xml.Linq;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_Apathe
{
    public class Connection_Database_Postgres
    {
        /// <summary>
        /// Создает подключение  доступное только в этом классе
        /// </summary>
        public string connectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=1";

        /// <summary>
        /// Создает подключение статическое
        /// </summary>
        public static string conectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=1";


        /// <summary>
        /// Создает таблицу  пользователь 
        /// </summary>
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


        /// <summary>
        /// Проверяет пользователя 
        /// </summary>
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


        /// <summary>
        ///Добавляет пользователя
        /// </summary>
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


        /// <summary>
        /// Добавляет logi 
        /// </summary>
        public void Insert_postgres(Connection_Database_Postgres _Postgres)
        {
            var path = Environment.CurrentDirectory.ToString();

            string pattern = @"^(.*?) - - \[(.*?)\] ""(.*?)"" (\d+) (\d+)$";

            // Создание регулярного выражения
            var regex = new Regex(pattern);

            using (var reader = new StreamReader(path + "\\access_log"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Поиск совпадений в строке лог-файла
                    var match = regex.Match(line);

                    // Обработка совпадений
                    if (match.Success)
                    {
                        var ip = match.Groups[1].Value;
                        var datetime = match.Groups[2].Value;
                        var request = match.Groups[3].Value;
                        var status = match.Groups[4].Value;
                        var size = match.Groups[5].Value;

                        // var sises = match.Groups[6].Value;
                        Console.WriteLine($"IP: {ip} Request: {request}");
                    
                        //     string dat = "28/Jul/2006:10:27:10 -0300";
                        string format = "dd/MMM/yyyy:HH:mm:ss zzz";

                        DateTime datetimes = DateTime.ParseExact(datetime, format, CultureInfo.InvariantCulture);
                        int CountLog = 0;
                        bool id_logs = false;
                        using (var connection = new NpgsqlConnection(_Postgres.connectionString))
                        {
                            connection.Open();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                var SqL = "SELECT Count(*) AS rec_count FROM logs";
                                //command.ExecuteNonQuery();
                                NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                                npgsqlCommand2.ExecuteNonQuery();

                                NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                                //  NpgsqlDataReader npgsqlDataReade = npgsqlCommand3.ExecuteReader();
                                SqL = "";
                                if (npgsqlDataReader.HasRows == true)
                                {
                                    while (npgsqlDataReader.Read())
                                    {
                                        CountLog = Convert.ToInt32(npgsqlDataReader["rec_count"]);
                                    }
                                    if (CountLog == 12)
                                    {
                                        id_logs = true;
                                    }
                                    else
                                    {
                                    }
                                }
                                else
                                {
                                    id_logs = false;
                                }      
                            }
                        }
                        if (id_logs == true)
                        {
                        }
                        else
                        {
                            using (var connections = new NpgsqlConnection(_Postgres.connectionString))
                            {
                                connections.Open();

                                // Вставка логов в таблицу
                                string Insert_logs = $"INSERT INTO logs (ip, date, request, status, size) VALUES ('{ip}','{datetimes:G}', '{request}','{status}', {size})";

                                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(Insert_logs, connections);
                                npgsqlCommand.ExecuteNonQuery();

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Выводит  список логов в Json[]
        /// </summary>
        public Json[] Select_logi(Connection_Database_Postgres _Postgres)
        {
            Json[] logs;
            int CountLog = 0;
            using (var connection = new NpgsqlConnection(_Postgres.connectionString))
            {
                connection.Open();
                using (MemoryStream ms = new MemoryStream())
                {
                    var SqL = "SELECT Count(*) AS rec_count FROM logs";
                    //command.ExecuteNonQuery();
                    NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                    npgsqlCommand2.ExecuteNonQuery();

                    NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                    //  NpgsqlDataReader npgsqlDataReade = npgsqlCommand3.ExecuteReader();
                    SqL = "";
                    if (npgsqlDataReader.HasRows == true)
                    {
                        while (npgsqlDataReader.Read())
                        {
                            CountLog = Convert.ToInt32(npgsqlDataReader["rec_count"]);
                        }
                    }
                    else
                    {
                    }
                }
            }

            Json[] log = new Json[CountLog];
            using (var connection = new NpgsqlConnection(_Postgres.connectionString))
            {
                connection.Open();
                using (MemoryStream ms = new MemoryStream())
                {
                    var SqL = "SELECT id_logs, ip, date, request, status, size FROM public.logs;";
                    //command.ExecuteNonQuery();
                    NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                    npgsqlCommand2.ExecuteNonQuery();

                    NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                    //  NpgsqlDataReader npgsqlDataReade = npgsqlCommand3.ExecuteReader();
                    SqL = "";
                    int i = 0;
                    if (npgsqlDataReader.HasRows == true)
                    {
                        while (npgsqlDataReader.Read())
                        {
                            Json Log = new Json(Convert.ToString(npgsqlDataReader["Ip"]),
                            Convert.ToDateTime(npgsqlDataReader["date"]),
                            Convert.ToString(npgsqlDataReader["request"]),
                            Convert.ToString(npgsqlDataReader["status"]),
                            Convert.ToString(npgsqlDataReader["size"]));
                            log[i] = Log;
                            i++;
                        }
                    }
                    else
                    {
                    }
                }
            }
            logs = log;
            return logs;
        }

    
     
        /// <summary>
        /// Выводит  список логов в Json[]
        /// </summary>
        public Json[] filter_logi(Connection_Database_Postgres postgres, string Ip, string dati1, string dati2,string Status)
        {
            Json[] logs;

            int CountLog = 0;
            using (var connection = new NpgsqlConnection(postgres.connectionString))
            {
                connection.Open();

                using (MemoryStream ms = new MemoryStream())
                {
                    string All = "";
                    if (string.IsNullOrEmpty(Ip) == false) { All = $"Ip Like '{Ip}'"; };
                    if (string.IsNullOrEmpty(dati1) == false)
                    {
                        if (All.Length > 0) { All = All + " and "; };
                        All = All + $"date >= '{dati1}'";
                    };
                    if (string.IsNullOrEmpty(dati2) == false) 
                    {
                        if (All.Length > 0) { All = All + " and "; };
                        All = All + $"date <= '{dati2}'"; 
                    };
                    if (string.IsNullOrEmpty(Status) == false) 
                    {
                        if (All.Length > 0) { All = All + " and "; };
                        All = $"status Like '{Status}'";           
                    };

                    if (All.Length > 0) { All = " where " + All; };

                    var SqL = $"SELECT Count(*) AS rec_count FROM logs "+All;
                    //command.ExecuteNonQuery();
                    NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                    npgsqlCommand2.ExecuteNonQuery();

                    NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                    //  NpgsqlDataReader npgsqlDataReade = npgsqlCommand3.ExecuteReader();
                  //  SqL = "";
                    if (npgsqlDataReader.HasRows == true)
                    {
                        while (npgsqlDataReader.Read())
                        {
                            CountLog = Convert.ToInt32(npgsqlDataReader["rec_count"]);
                        }
                    }
                    else
                    {
                    }
                }
            }
            Json[] log = new Json[CountLog];

            using (var connection = new NpgsqlConnection(postgres.connectionString))
            {
                connection.Open();

                using (MemoryStream ms = new MemoryStream())
                {
                    string All = "";
                    if (string.IsNullOrEmpty(Ip) == false) { All = $"Ip Like '{Ip}'"; };
                    if (string.IsNullOrEmpty(dati1) == false)
                    {
                        if (All.Length > 0) { All = All + " and "; };
                        All = All + $"date >= '{dati1}'";
                    };
                    if (string.IsNullOrEmpty(dati2) == false)
                    {
                        if (All.Length > 0) { All = All + " and "; };
                        All = All + $"date <= '{dati2}'";
                    };
                    if (string.IsNullOrEmpty(Status) == false)
                    {
                        if (All.Length > 0) { All = All + " and "; };
                        All = $"status Like '{Status}'";
                    };

                    if (All.Length > 0) { All = " where " + All; };

                    var SqL = $"SELECT id_logs, ip, date, request, status, size  FROM logs " + All;
                    //command.ExecuteNonQuery();
                    NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                    npgsqlCommand2.ExecuteNonQuery();

                    NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                    //  NpgsqlDataReader npgsqlDataReade = npgsqlCommand3.ExecuteReader();
                    //  SqL = "";
                    SqL = "";
                    int i = 0;
                    if (npgsqlDataReader.HasRows == true)
                    {
                        while (npgsqlDataReader.Read())
                        {
                            Json Log = new Json(Convert.ToString(npgsqlDataReader["Ip"]),
                             Convert.ToDateTime(npgsqlDataReader["date"]),
                              Convert.ToString(npgsqlDataReader["request"]),
                              Convert.ToString(npgsqlDataReader["status"]),
                               Convert.ToString(npgsqlDataReader["size"]));
                            log[i] = Log;
                            i++;
                        }
                    }
                    else
                    {
                    }
                }
            }
            logs = log;
            return logs;
        }

        ///  
        //using (FileStream json_Fille = new FileStream(path + "\\data.json", FileMode.Append))
        //{
        //    if (json_Fille.Length == 0)
        //    {
        //        Byte[] Aa = Encoding.Default.GetBytes("[");
        //        json_Fille.Write(Aa, 0, Aa.Length);
        //    }
        //    var options = new JsonSerializerOptions
        //    {
        //        AllowTrailingCommas = true
        //    };

        //    Json json = new Json(ip, datetimes, request, status, size);
        //    JsonSerializer.Serialize<Json>(json_Fille, json, options);
        //    if (CountLog < 11)
        //    {
        //        Byte[] Aa2 = Encoding.Default.GetBytes(",");
        //        json_Fille.Write(Aa2, 0, Aa2.Length);
        //    }
        //    if (CountLog == 11)
        //    {
        //        Byte[] Aa3 = Encoding.Default.GetBytes("]");
        //        json_Fille.Write(Aa3, 0, Aa3.Length);
        //    }
        //}
        //long endPoint = json_Fille.Length;
        // Set the stream position to the end of the file.        
        // json_Fille.Seek(endPoint, SeekOrigin.Begin);

        //Что-то падает
        //    npgsqlCommand.CommandText = Insert_logs;
        //Count = 0;


        /////////////////
    }
}
