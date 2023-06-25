//using Npgsql;
//using System;

namespace Api_Apathe
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }


        public Connection_Database_Postgres _Postgres = new Connection_Database_Postgres();

        private void Form1_Load(object sender, EventArgs e)
        {
            _Postgres.Create_Table_User(_Postgres);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                //Есть ли значение пароль
                if (textBox3.Text == "")
                {
                    //присваеваем пароль   
                    textBox3.Text = textBox3.Text;
                    //Маскируем пароль *
                    textBox3.PasswordChar = '*';
                }
                else
                {
                    textBox3.Text = string.Empty;
                    textBox3.UseSystemPasswordChar = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox1.Text == "" && textBox3.Text == "")
                {
                    MessageBox.Show("Логин и пароль не заполнен!");
                }
                else
                {
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("Логин не заполнен!");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(textBox3.Text))
                        {
                            MessageBox.Show("Пароль не заполнен!");
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
                                MessageBox.Show("Такой учетной записи нету!");
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
    }
}