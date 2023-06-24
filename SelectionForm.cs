using Npgsql;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Api_Apathe
{
    public partial class SelectionForm : Form
    {
        public SelectionForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Json[] logs;
        public Connection_Database_Postgres Postgres;

        public void Connect(Connection_Database_Postgres postgres)
        {
            Postgres = postgres;

        }

        public void Servis()
        {
            try
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

                            // Создание базы данных
                            //     string dat = "28/Jul/2006:10:27:10 -0300";
                            string format = "dd/MMM/yyyy:HH:mm:ss zzz";

                            DateTime datetimes = DateTime.ParseExact(datetime, format, CultureInfo.InvariantCulture);
                            int CountLog = 0;
                            bool id_logs = false;
                            using (var connection = new NpgsqlConnection(Connection_Database_Postgres.conectionString))
                            {
                                connection.Open();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    //using (FileStream json_Fille = new FileStream("data.json", FileMode.OpenOrCreate))
                                    //{
                                    var SqL = "SELECT Count(*) AS rec_count FROM logs";
                                    //command.ExecuteNonQuery();
                                    NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                                    npgsqlCommand2.ExecuteNonQuery();
                                    //NpgsqlCommand npgsqlCommand = new NpgsqlCommand(SqL, connection);
                                    //npgsqlCommand.ExecuteNonQuery();
                                    ///NpgsqlCommand npgsqlCommand3 = new NpgsqlCommand(SqL, connection);
                                    //npgsqlCommand3.ExecuteNonQuery();

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
                                    //}
                                }
                            }


                            // Что -то пошло не так
                            if (id_logs == true)
                            {

                            }
                            else
                            {
                                using (FileStream json_Fille = new FileStream(path + "\\data.json", FileMode.Append))
                                {
                                    using (var connections = new NpgsqlConnection(Connection_Database_Postgres.conectionString))
                                    {
                                        connections.Open();
                                        var options = new JsonSerializerOptions
                                        {
                                            AllowTrailingCommas = true
                                        };

                                        if (json_Fille.Length == 0)
                                        {
                                            Byte[] Aa = Encoding.Default.GetBytes("[");
                                            json_Fille.Write(Aa, 0, Aa.Length);
                                        }

                                        //long endPoint = json_Fille.Length;
                                        // Set the stream position to the end of the file.        
                                        // json_Fille.Seek(endPoint, SeekOrigin.Begin);

                                        Json json = new Json(ip, datetimes, request, status, size);
                                        JsonSerializer.Serialize<Json>(json_Fille, json, options);
                                        if (CountLog < 11)
                                        {
                                            Byte[] Aa2 = Encoding.Default.GetBytes(",");
                                            json_Fille.Write(Aa2, 0, Aa2.Length);
                                        }
                                        // Вставка логов в таблицу
                                        string Insert_logs = $"INSERT INTO logs (ip, date, request, status, size) VALUES ('{ip}','{datetimes:G}', '{request}','{status}', {size})";

                                        NpgsqlCommand npgsqlCommand = new NpgsqlCommand(Insert_logs, connections);

                                        //Что-то падает
                                        //    npgsqlCommand.CommandText = Insert_logs;
                                        npgsqlCommand.ExecuteNonQuery();
                                    }
                                    if (CountLog == 11)
                                    {
                                        Byte[] Aa3 = Encoding.Default.GetBytes("]");
                                        json_Fille.Write(Aa3, 0, Aa3.Length);
                                    }
                                    //Count = 0;
                                }
                            }
                        }

                    }

                    int CountLogs = 0;

                    using (var connection = new NpgsqlConnection(Connection_Database_Postgres.conectionString))
                    {
                        connection.Open();

                        var SqL = "SELECT Count(*) AS rec_count FROM logs";

                        NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                        npgsqlCommand2.ExecuteNonQuery();
                        //NpgsqlCommand npgsqlCommand = new NpgsqlCommand(SqL, connection);
                        //npgsqlCommand.ExecuteNonQuery();
                        ///NpgsqlCommand npgsqlCommand3 = new NpgsqlCommand(SqL, connection);
                        //npgsqlCommand3.ExecuteNonQuery();

                        NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                        SqL = "";
                        if (npgsqlDataReader.HasRows == true)
                        {
                            while (npgsqlDataReader.Read())
                            {
                                CountLogs = Convert.ToInt32(npgsqlDataReader["rec_count"]);
                            }

                        }
                        else
                        {

                        }
                    }

                    using (FileStream json_Fille = new FileStream(path + "\\data.json", FileMode.OpenOrCreate))
                    {
                        //    int count = 0;
                        //    byte[] buffer = new byte[1024];
                        //   count =  json_Fille.Read(buffer, 0, buffer.Length);



                        //     Json_T log = JsonSerializer.Deserialize<Json_T>(json_Fille);
                        JObject log = JObject.Parse(json_Fille);
                        if (log == null)
                        {

                        }
                        else
                        {
                          

                            Json[] jsons = new Json[CountLogs];
                            //for (int i = 0; i < jsons.Length; i++)
                            //{
                            //    jsons[i] = log;
                            //}
                            // jsons[Count] = log;

                          //  logs = log;
                            jsons = null;
                        }

                    }

                    //Колонку по умолчанию сдесь упало
                    for (int i = 1; i < logs.Length; i++)
                    {
                        DataGridViewTextBoxCell Ip = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[0];
                        Ip.Value = logs[i].Ip;

                        DataGridViewTextBoxCell Date = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                        Date.Value = logs[i].Date;

                        DataGridViewTextBoxCell Request = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[2];
                        Request.Value = logs[i].Request;
                        DataGridViewTextBoxCell Status = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[3];
                        Status.Value = logs[i].Status;

                        DataGridViewTextBoxCell Size = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[4];
                        Size.Value = logs[i].Size;
                    }

                    //Добоавляем колонку друзей
                    //DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
                    ////Называем колонку для картинок
                    //imgColumn.Name = "Images";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Servis();
        }

        private void Form_close(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Servis();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
