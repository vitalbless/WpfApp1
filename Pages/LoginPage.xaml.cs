using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data.Context;
using WpfApp1.Data.Models;

namespace WpfApp1.Pages
{
    public partial class LoginPage : Page
    {
        private ApplicationContext db;

        public LoginPage()
        {
            InitializeComponent();
            db = new ApplicationContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;

            var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                MessageBox.Show("Вы успешно вошли!");
                NavigationService.Navigate(new PageMain(user));  // Передача текущего пользователя на главную страницу
            }
            else
            {
                MessageBox.Show("Не правильный логин или пароль!");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = RegisterUsernameTextBox.Text;
            string password = RegisterPasswordBox.Password;
            string roleName = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Пожалуйста заполните все поля!");
                return;
            }

            if (db.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Имя пользователя уже занято!");
                return;
            }

            var role = db.Roles.FirstOrDefault(r => r.RoleName == roleName);
            if (role == null)
            {
                MessageBox.Show("Роль не найдена!");
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                RoleId = role.RoleId
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            MessageBox.Show("Регистрация прошла успешно!");
        }
    }
}
