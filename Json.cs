
namespace Api_Apathe
{
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
    [Serializable]
    public class Json_Travel
    {
        public List<Json> Json_Travels { get; set; } = new List<Json>();
    }
    [Serializable]
    public class Json_T
    {
        Json_T() { }
        public Json[] Bot_Telegram { get; set; }
        public Json_T(Json[] bot_Telegram)
        {
            Bot_Telegram = bot_Telegram;
        }
    }
}
