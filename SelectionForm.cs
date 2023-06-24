﻿using Npgsql;
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
        
                // dataGridView1.RowCount = data.Length;
                //Колонку по умолчанию
               // dataGridView1.ColumnCount = 0;
                DataGridViewTextBoxColumn Ips = new DataGridViewTextBoxColumn();
                Ips.Name = "Ip adress";
                DataGridViewTextBoxColumn Dats = new DataGridViewTextBoxColumn();
                Dats.Name = "Дата";

                DataGridViewTextBoxColumn Requests = new DataGridViewTextBoxColumn();
                Requests.Name = "Запросы";

                DataGridViewTextBoxColumn Statuss = new DataGridViewTextBoxColumn();
                Statuss.Name = "Статус";


                DataGridViewTextBoxColumn Sizes = new DataGridViewTextBoxColumn();
                Sizes.Name = "Размер";

               // dataGridView1.Columns.Count = 0;

                dataGridView1.Columns.Add(Ips);

                dataGridView1.Columns.Add(Dats);

                dataGridView1.Columns.Add(Requests);

                dataGridView1.Columns.Add(Statuss);

                dataGridView1.Columns.Add(Sizes);

                dataGridView1.RowCount = data.Count();

                for (int i = 0; i < data.Length; i++)
                {
                   DataGridViewTextBoxCell Ip = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Ip adress"];
                   Ip.Value = data[i].Ip;
              


                    DataGridViewTextBoxCell Request = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Запросы"];
                    Request.Value = data[i].Request;
                   
                    DataGridViewTextBoxCell Status = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Статус"];
                    Status.Value = data[i].Status;
                
                    DataGridViewTextBoxCell Size = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Размер"];
                    Size.Value = data[i].Size;
                   // Size.Size. =AutoSizeMode  .Zoom;
                }
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                //DataGridViewTextBoxColumn Dat = new DataGridViewTextBoxColumn();
                //Dat.Name = "Дата";
                //dataGridView1.Columns.Add(Dat);

                //dataGridView1.RowCount = data.Count();
                //for (int i = 0; i < data.Count(); i++)
                //{
                //    // DataGridViewTextBoxCell

                //  //  dataGridView1.Rows[0].Cells[0]
                ////}


                //dataGridView1.RowCount = data.Count();
                //for (int i = 0; i < data.Length; i++)
                //{

                //}


                //dataGridView1.RowCount = data.Count();
                //for (int i = 0; i < data.Length; i++)
                //{

                //}

                //for (int i = 0; i < data.Length; i++)
                //{

                //}


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
