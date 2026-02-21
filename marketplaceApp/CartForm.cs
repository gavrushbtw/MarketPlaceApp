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
    public partial class CartForm : Form
    {
        private ListView cartList;
        private Label totalLabel;
        DatabaseHelper db = new DatabaseHelper();

        public CartForm()
        {
            InitializeCart();
            LoadCartFromDatabase(); // ✅ Вызываем здесь
        }

        private void InitializeCart()
        {
            this.Text = "Корзина";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label()
            {
                Text = "Корзина покупок",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Size = new Size(200, 40),
                Location = new Point(20, 20)
            };

            // Список товаров в корзине
            cartList = new ListView()
            {
                Location = new Point(20, 70),
                Size = new Size(650, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            cartList.Columns.Add("Товар", 250);
            cartList.Columns.Add("Цена", 100);
            cartList.Columns.Add("Кол-во", 80);
            cartList.Columns.Add("Сумма", 120);

            // Итого
            totalLabel = new Label()
            {
                Text = "Итого: 0 руб.",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(200, 30),
                Location = new Point(20, 390),
                ForeColor = Color.FromArgb(0, 100, 200)
            };

            // Кнопки
            Button checkoutBtn = new Button()
            {
                Text = "Оформить заказ",
                Size = new Size(200, 40),
                Location = new Point(470, 390),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            checkoutBtn.Click += (s, e) =>
            {
                CheckoutForm checkout = new CheckoutForm();
                checkout.Show();
            };

            this.Controls.AddRange(new Control[] { title, cartList, totalLabel, checkoutBtn });
        }

        // ✅ Метод загрузки корзины из БД
        private void LoadCartFromDatabase()
        {
            try
            {
                cartList.Items.Clear();
                decimal totalAmount = 0;
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT т.НазваниеТовара, т.Цена, к.Количество, (т.Цена * к.Количество) as Сумма
                               FROM Корзина к
                               JOIN Товары т ON к.ID_товара = т.ID_товара
                               WHERE к.ID_пользователя = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserSession.CurrentUserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string productName = reader.GetString(0);
                                decimal price = reader.GetDecimal(1);
                                int quantity = reader.GetInt32(2);
                                decimal sum = reader.GetDecimal(3);

                                ListViewItem item = new ListViewItem(productName);
                                item.SubItems.Add($"{price} руб.");
                                item.SubItems.Add(quantity.ToString());
                                item.SubItems.Add($"{sum} руб.");

                                cartList.Items.Add(item);
                                totalAmount += sum;
                            }
                        }
                    }
                }


                totalLabel.Text = $"Итого: {totalAmount} руб.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки корзины: " + ex.Message);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CartForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "CartForm";
            this.Load += new System.EventHandler(this.CartForm_Load);
            this.ResumeLayout(false);

        }

        private void CartForm_Load(object sender, EventArgs e)
        {

        }
    }
}
