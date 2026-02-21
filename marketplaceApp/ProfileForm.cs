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
    public partial class ProfileForm : Form
    {
        DatabaseHelper db = new DatabaseHelper();
        public ProfileForm()
        {
            InitializeProfile();
            LoadUserProfile(); // ✅ Вызываем здесь
        }

        private void InitializeProfile()
        {
            this.Text = "Мой профиль";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label()
            {
                Text = "Мой профиль",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Size = new Size(200, 40),
                Location = new Point(20, 20)
            };

            this.Controls.Add(title);
        }

        // ✅ Метод загрузки профиля из БД
        private void LoadUserProfile()
        {
            try
            {
                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT ФИО, ЭлектроннаяПочта, Адрес, Телефон FROM Пользователи WHERE ID_пользователя = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserSession.CurrentUserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = reader.GetString(0);
                                string email = reader.GetString(1);
                                string address = reader.GetString(2);
                                string phone = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3);

                                CreateProfileControls(fullName, email, address, phone);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки профиля: " + ex.Message);
            }
        }

        private void CreateProfileControls(string fullName, string email, string address, string phone)
        {
            int y = 80;
            string[] labels = { "ФИО:", "Email:", "Адрес:", "Телефон:" };
            string[] values = { fullName, email, address, phone };

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label()
                {
                    Text = labels[i],
                    Location = new Point(30, y),
                    Size = new Size(80, 25),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                TextBox txt = new TextBox()
                {
                    Text = values[i],
                    Location = new Point(120, y),
                    Size = new Size(300, 25),
                    Font = new Font("Segoe UI", 10),
                    ReadOnly = true,
                    BackColor = Color.WhiteSmoke
                };

                this.Controls.Add(lbl);
                this.Controls.Add(txt);
                y += 40;
            }

            // Кнопка редактирования
            Button editBtn = new Button()
            {
                Text = "Редактировать профиль",
                Size = new Size(200, 35),
                Location = new Point(150, y + 20),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            editBtn.Click += (s, e) =>
            {
                MessageBox.Show("Функция редактирования будет реализована позже");
            };

            this.Controls.Add(editBtn);
        }
    }
}
