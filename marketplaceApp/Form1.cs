using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace marketplaceApp
{
    public partial class Form1 : Form
    {
        DatabaseHelper db = new DatabaseHelper();
        public Form1()
        {
            this.FormClosing += (s, e) => Application.Exit();
            InitializeComponent();
            this.CenterToScreen();
            this.SetGradientBackground(
                Color.FromArgb(255, 183, 77), 
                Color.FromArgb(255, 138, 101) 
            );
            db.TestConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            string email = textBox1.Text;
            string password = textBox2.Text;

            try
            {
                using (NpgsqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT \"ID_пользователя\", \"ФИО\", \"Роль\" FROM \"Пользователи\" WHERE \"ЭлектроннаяПочта\" = @Email AND \"Пароль\" = @Password";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserSession.CurrentUserID = reader.GetInt32(0);
                                UserSession.CurrentUserName = reader.GetString(1);
                                UserSession.CurrentUserRole = reader.GetString(2);

                                MessageBox.Show($"Добро пожаловать, {UserSession.CurrentUserName}! Роль: {UserSession.CurrentUserRole}");
                                Logger.Log($"Успешный вход: {UserSession.CurrentUserName}, Роль: {UserSession.CurrentUserRole}");

                                Navigation mainForm = new Navigation();
                                mainForm.Show();
                                this.Hide();
                                sw.Stop();
                                Logger.Log($"Время входа в систему: {sw.ElapsedMilliseconds} мс");
                            }
                            else
                            {
                                MessageBox.Show("Неверный email или пароль");
                                Logger.Log($"Неудачная попытка входа: {email}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"ОШИБКА входа: {ex.Message}");
                MessageBox.Show("Ошибка входа: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
