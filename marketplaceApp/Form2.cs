using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace marketplaceApp
{
    public partial class Form2 : Form
    {
        DatabaseHelper db = new DatabaseHelper();
        public Form2()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.SetGradientBackground(
                Color.FromArgb(255, 183, 77),
                Color.FromArgb(245, 124, 0)   
            );
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox3.Text;
            string fullName = textBox2.Text;
            string address = textBox4.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            try
            {
                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO Пользователи (ЭлектроннаяПочта, Пароль, ФИО, Адрес) 
                               VALUES (@Email, @Password, @FullName, @Address)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password); // В реальном приложении хэшируйте пароль!
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Address", address);

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Регистрация успешна!");
                            this.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Ошибка уникальности
                {
                    MessageBox.Show("Пользователь с таким email уже существует");
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации: " + ex.Message);
                }
            }
        }
    }
}
