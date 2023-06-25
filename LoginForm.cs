//using Npgsql;
//using System;

using System.Net;
using System.Text.Json;

namespace Api_Apathe
{
    public partial class LoginForm : Form
    {

       public static string User_postgres;


        public static string Postgres_password;


        public static string Name_Sql;
        /// <summary>
        /// ����������� ���������
        /// </summary>
        /// <param name="args"></param>
        public LoginForm(string[] args)
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


        public Connection_Database_Postgres _Postgres = new Connection_Database_Postgres();

        /// <summary>
        /// ������������ ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
             Settings();
            _Postgres.Create_Table_User(_Postgres);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                //���� �� �������� ������
                if (textBox3.Text == "")
                {
                    //����������� ������   
                    textBox3.Text = textBox3.Text;
                    //��������� ������ *
                    textBox3.PasswordChar = '*';
                }
                else
                {
                    textBox3.Text = string.Empty;
                    textBox3.UseSystemPasswordChar = true;
                }
            }
        }

        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" && textBox3.Text == "")
                {
                    MessageBox.Show("����� � ������ �� ��������!");
                }
                else
                {
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("����� �� ��������!");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(textBox3.Text))
                        {
                            MessageBox.Show("������ �� ��������!");
                        }
                        else
                        {
                            bool User = false;
                            User = _Postgres.Check_user(User, _Postgres, textBox1.Text, textBox3.Text);

                            if (User == true)
                            {
                                SelectionForm form2 = new SelectionForm();
                                form2.Show();
                                form2.Connect(_Postgres);
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("����� ������� ������ ����!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //�����������
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RegistrationForm form = new RegistrationForm();
                form.Connect(_Postgres);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// ��������� log �
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings()
        {

            try
            {
                string path = Environment.CurrentDirectory.ToString();

                FileInfo fileInfo = new FileInfo(path + "\\Server.json");
                if (fileInfo.Exists)
                {
                    using (FileStream fs = new FileStream("Server.json", FileMode.Open))
                    {
                        Settings _aFile = JsonSerializer.Deserialize<Settings>(fs);
                        User_postgres = _aFile.Postgres_user;
                        Postgres_password = _aFile.Postgres_password;
                        Name_Sql = _aFile.Database;
                        _Postgres.connectionString = $"Host=localhost;Port=5432;Database={Name_Sql};Username={User_postgres};Password={Postgres_password}";

                    }
                }
                else
                {
                    string pattern = @"^(.*?) - - \[(.*?)\] ""(.*?)"" (\d+) (\d+)$";
                    using (FileStream fileStream = new FileStream("Server.json", FileMode.OpenOrCreate))
                    {
                        Settings connect_Server_ = new Settings("postgres", "1", "localhost", 5432 , "usersdb", path, pattern);
                        JsonSerializer.Serialize<Settings>(fileStream, connect_Server_);

                    }

                    using (FileStream fileStream = new FileStream("Server.json", FileMode.OpenOrCreate))
                    {
                        Settings aFile = JsonSerializer.Deserialize<Settings>(fileStream);
                           User_postgres = aFile.Postgres_user;
                          Postgres_password = aFile.Postgres_password;
                           Name_Sql = aFile.Database;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}