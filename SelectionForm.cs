using System.Net;
using System.Text.Json;
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
        /// <summary>
        /// Класс для  взаимодействия с субд
        /// </summary>
        public Connection_Database_Postgres Postgres;


        /// <summary>
        /// Получает класс для взаимодейсвиям с данными 
        /// </summary>
        /// <param name="postgres"></param>
        public void Connect(Connection_Database_Postgres postgres)
        {
            Postgres = postgres;
        }


        /// <summary>
        /// Делает запрос к субд и выводит 
        /// </summary>
        public void Servis()
        {
            try
            {
                Postgres.Insert_postgres(Postgres);
                var data = Postgres.Select_logi(Postgres);
                //Колонку по умолчанию
                // dataGridView1.ColumnCount = 0;
                ////Создаем колонки
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
                /////
                //Определяем колонки с начала
                dataGridView1.ColumnCount = 0;
                ////Добавляем колонки для отображаения
                dataGridView1.Columns.Add(Ips);
                dataGridView1.Columns.Add(Dats);
                dataGridView1.Columns.Add(Requests);
                dataGridView1.Columns.Add(Statuss);
                dataGridView1.Columns.Add(Sizes);
                dataGridView1.RowCount = data.Count();
                ////


                ////Отображаеться данные 
                for (int i = 0; i < data.Length; i++)
                {
                    DataGridViewTextBoxCell Ip = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Ip adress"];
                    Ip.Value = data[i].Ip;
                    DataGridViewTextBoxCell time = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Дата"];
                    time.Value = data[i].Date;
                    DataGridViewTextBoxCell Request = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Запросы"];
                    Request.Value = data[i].Request;
                    DataGridViewTextBoxCell Status = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Статус"];
                    Status.Value = data[i].Status;
                    DataGridViewTextBoxCell Size = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Размер"];
                    Size.Value = data[i].Size;
                }
                //Для серилизации
                logs = data;
                //Автоматически  задается размер колонкам
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Загружаеться форма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Закрывает приложение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_close(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Загружает log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Servis();
        }

        /// <summary>
        /// Фильтрация данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                var Filter_data = Postgres.filter_logi(Postgres, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);

                //Проверяет есть-ли данные после фильтрации
                bool ip = false;
                bool Date = false;
                bool Request = false;
                bool Statusss = false;
                bool Siz = false;

                dataGridView1.ColumnCount = 0;

                for (int i = 0; i < Filter_data.Length; i++)
                {
                    if (string.IsNullOrEmpty(Filter_data[i].Ip))
                    {
                    }
                    else
                    {
                        DataGridViewTextBoxColumn Ips = new DataGridViewTextBoxColumn();
                        Ips.Name = "Ip adress";
                        dataGridView1.Columns.Add(Ips);
                        ip = true;
                        break;
                    }
                }

                for (int i = 0; i < Filter_data.Length; i++)
                {
                    if (string.IsNullOrEmpty(Filter_data[i].Date.ToString()))
                    {
                    }
                    else
                    {
                        DataGridViewTextBoxColumn Dats = new DataGridViewTextBoxColumn();
                        Dats.Name = "Дата";
                        dataGridView1.Columns.Add(Dats);
                        Date = true;
                        break;
                    }
                }
                for (int i = 0; i < Filter_data.Length; i++)
                {
                    if (string.IsNullOrEmpty(Filter_data[i].Request))
                    {
                    }
                    else
                    {
                        DataGridViewTextBoxColumn Requests = new DataGridViewTextBoxColumn();
                        Requests.Name = "Запросы";
                        dataGridView1.Columns.Add(Requests);
                        Request = true;
                        break;
                    }
                }
                for (int i = 0; i < Filter_data.Length; i++)
                {
                    if (string.IsNullOrEmpty(Filter_data[i].Status))
                    {
                    }
                    else
                    {
                        DataGridViewTextBoxColumn Statuss = new DataGridViewTextBoxColumn();
                        Statuss.Name = "Статус";
                        dataGridView1.Columns.Add(Statuss);
                        Statusss = true;
                        break;
                    }
                }

                for (int i = 0; i < Filter_data.Length; i++)
                {
                    if (string.IsNullOrEmpty(Filter_data[0].Size))
                    {
                    }
                    else
                    {
                        DataGridViewTextBoxColumn Sizes = new DataGridViewTextBoxColumn();
                        Sizes.Name = "Размер";
                        dataGridView1.Columns.Add(Sizes);
                        Siz = true;
                        break;
                    }
                }


                dataGridView1.RowCount = Filter_data.Count();
                for (int i = 0; i < Filter_data.Length; i++)
                {
                    if (ip == true)
                    {
                        DataGridViewTextBoxCell Ip = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Ip adress"];
                        Ip.Value = Filter_data[i].Ip;
                    }
                    if (Date == true)
                    {
                        DataGridViewTextBoxCell time = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Дата"];
                        time.Value = Filter_data[i].Date;
                    }
                    if (Request == true)
                    {
                        DataGridViewTextBoxCell Requests = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Запросы"];
                        Requests.Value = Filter_data[i].Request;
                    }
                    if (Statusss == true)
                    {
                        DataGridViewTextBoxCell Status = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Статус"];
                        Status.Value = Filter_data[i].Status;
                    }
                    if (Siz == true)
                    {
                        DataGridViewTextBoxCell Size = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells["Размер"];
                        Size.Value = Filter_data[i].Size;
                    }
                }
                //Для серилизации
                logs = Filter_data;
                //Автоматически  задается размер колонкам
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Выгрузить log в
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {   //Путь
                var path = Environment.CurrentDirectory.ToString();
                //Создаеться и серилизуеться файл 
                using (FileStream json_Fille = new FileStream(path + "\\data.json", FileMode.OpenOrCreate))
                {
                    Json_T json = new Json_T(logs);
                    //Серилизация Json_T
                    JsonSerializer.Serialize<Json_T>(json_Fille, json);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
     
    }
}
