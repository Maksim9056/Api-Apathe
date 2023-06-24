using Npgsql;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using System.Text;
//using Newtonsoft.Json.Linq;

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
                Postgres.Insert_postgres(Postgres);

                 var data  =  Postgres.Select_logi(Postgres);
                for (int i = 1; i < data.Length; i++)
                {
                    DataGridViewTextBoxCell Ip = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[0];
                    Ip.Value = data[i].Ip;

                    DataGridViewTextBoxCell Date = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    Date.Value = data[i].Date;

                    DataGridViewTextBoxCell Request = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[2];
                    Request.Value = data[i].Request;
                    DataGridViewTextBoxCell Status = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[3];
                    Status.Value = data[i].Status;

                    DataGridViewTextBoxCell Size = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[4];
                    Size.Value = data[i].Size;
                }



                //int CountLogs = 0;

                //using (var connection = new NpgsqlConnection(Connection_Database_Postgres.conectionString))
                //{
                //    connection.Open();

                //    var SqL = "SELECT Count(*) AS rec_count FROM logs";

                //    NpgsqlCommand npgsqlCommand2 = new NpgsqlCommand(SqL, connection);
                //    npgsqlCommand2.ExecuteNonQuery();
                //    //NpgsqlCommand npgsqlCommand = new NpgsqlCommand(SqL, connection);
                //    //npgsqlCommand.ExecuteNonQuery();
                //    ///NpgsqlCommand npgsqlCommand3 = new NpgsqlCommand(SqL, connection);
                //    //npgsqlCommand3.ExecuteNonQuery();

                //    NpgsqlDataReader npgsqlDataReader = npgsqlCommand2.ExecuteReader();

                //    SqL = "";
                //    if (npgsqlDataReader.HasRows == true)
                //    {
                //        while (npgsqlDataReader.Read())
                //        {
                //            CountLogs = Convert.ToInt32(npgsqlDataReader["rec_count"]);
                //        }

                //    }
                //    else
                //    {

                //    }
                //}

                //using (FileStream json_Fille = new FileStream(path + "\\data.json", FileMode.OpenOrCreate))
                //{
                //    int count = 0;
                //    byte[] buffer = new byte[1024];
                //   count =  json_Fille.Read(buffer, 0, buffer.Length);



                //     Json_T log = JsonSerializer.Deserialize<Json_T>(json_Fille);
                //  JObject log = JObject.Parse(json_Fille);
                //    if (log == null)
                //    {

                //    }
                //    else
                //    {


                //        Json[] jsons = new Json[CountLogs];
                //        //for (int i = 0; i < jsons.Length; i++)
                //        //{
                //        //    jsons[i] = log;
                //        //}
                //        // jsons[Count] = log;

                //      //  logs = log;
                //        jsons = null;
                //    }

                //}

                //Колонку по умолчанию сдесь упало


                //Добоавляем колонку друзей
                //DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
                ////Называем колонку для картинок
                //imgColumn.Name = "Images";
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
