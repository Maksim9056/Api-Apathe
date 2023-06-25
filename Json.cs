
namespace Api_Apathe
{    
    
    //Json класс  логов apathe
    [Serializable]
    public class Json
    {
        Json() { }
        public string Ip { get; set; }
        public DateTime Date { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }
        public string Size { get; set; }
        //public Json() { }
        public Json(string ip, DateTime date, string request, string status, string size)
        {
            Ip = ip;
            Date = date;
            Request = request;
            Status = status;
            Size = size;
        }
    }


    //Json_Travel для десерелизации Json_Travel класса
    [Serializable]
    public class Json_Travel
    {
        public List<Json> Json_Travels { get; set; } = new List<Json>();
    }


    //Json_Travel для Серелизации Json класа
    [Serializable]
    public class Json_T
    {
        Json_T() { }
        public Json[] Log_Apathe { get; set; }
        public Json_T(Json[] log_Apathe)
        {
            Log_Apathe = log_Apathe;
        }
    }



    public class Settings
    {  public Settings() { }

        public string Postgres_user { get; set; }
        public string Postgres_password { get; set; }  
        public string Ip { get; set; }
        public string Database { get; set; }
        public int  Postgres_Port { get; set; }
        public string Path_system { get; set; }
        public string Pattern_Fille_log { get; set; }

      
       // public string Database { get; set; }
        public Settings(string postgres_user, string postgres_password,string ip ,int port , string database, string path_system, string pattern_Fille_log)
        {
            Postgres_user = postgres_user;

            Postgres_password = postgres_password;

            Ip= ip;

            Database = database;
            Postgres_Port = port;

            Path_system = path_system;

            Pattern_Fille_log  = pattern_Fille_log;
        }
       
            

    }
}
