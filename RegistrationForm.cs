//using System.Windows.Forms;
namespace Api_Apathe
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
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

        public Connection_Database_Postgres Postgres;

        public void Connect(Connection_Database_Postgres postgres)
        { 
            Postgres = postgres;
        }

        private void Регистрация_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Пароль не заполнен!");
                }
                else
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        bool Users = false;

                        if (string.IsNullOrEmpty(textBox1.Text))
                        {
                            MessageBox.Show("Логин не заполнен!");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(textBox2.Text))
                            {
                                MessageBox.Show("Пароль не заполнен!");
                            }
                            else
                            {
                                Users = Postgres.Insert_user(Users, Postgres, textBox1.Text, textBox2.Text);

                                if (Users == true)
                                {
                                    MessageBox.Show("Добавление пользователя разрешено");
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Пользователь не добавлен!");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Пароли не совпадают !");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }
    }
}
