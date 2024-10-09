using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data.Context;
using WpfApp1.Data.Models;

namespace WpfApp1.Pages
{
    public partial class PageMain : Page
    {
        private ApplicationContext db;
        private User currentUser;

        public PageMain(User user)
        {
            InitializeComponent();
            db = new ApplicationContext();
            currentUser = user;
            LoadServiceRequests();
            ConfigureVisibilityBasedOnRole();
        }

        private void LoadServiceRequests()
        {
            RequestsListView.ItemsSource = db.ServiceRequests
                                              .Include(sr => sr.CreatedBy)
                                              .Include(sr => sr.AssignedTo)
                                              .ToList();
        }

        private void ConfigureVisibilityBasedOnRole()
        {
            // Hide create request group box for Technicians
            CreateRequestGroupBox.Visibility = currentUser.Role.RoleName == "Technician" ? Visibility.Collapsed : Visibility.Visible;

            // Show edit request group box for Technicians and Administrators
            EditRequestGroupBox.Visibility = currentUser.Role.RoleName != "Operator" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var newRequest = new ServiceRequest
            {
                Description = DescriptionTextBox.Text,
                Status = "Pending",
                TechnicianComments = string.Empty,  // Убедитесь, что поле не пустое
                CreatedById = currentUser.UserId,
                CreatedAt = DateTime.Now
            };

            db.ServiceRequests.Add(newRequest);
            db.SaveChanges();
            LoadServiceRequests();

            MessageBox.Show("Service request created successfully!");
        }

        private void RequestsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, является ли выбранный элемент запросом на обслуживание
            if (RequestsListView.SelectedItem is ServiceRequest selectedRequest)
            {
                // Устанавливаем выбранный элемент в ComboBox для статуса
                StatusComboBox.SelectedItem = StatusComboBox.Items
                    .Cast<ComboBoxItem>() // Преобразуем элементы ComboBox в тип ComboBoxItem
                    .FirstOrDefault(item => item.Content.ToString() == selectedRequest.Status); // Находим первый элемент, статус которого совпадает с выбранным запросом

                // Устанавливаем комментарии техника в соответствующее текстовое поле
                TechnicianCommentsTextBox.Text = selectedRequest.TechnicianComments;

                // Определяем, можно ли редактировать поля (если текущий пользователь не оператор)
                bool isEditable = currentUser.Role.RoleName != "Operator";

                // Включаем или отключаем возможность редактирования полей в зависимости от роли пользователя
                StatusComboBox.IsEnabled = isEditable;
                TechnicianCommentsTextBox.IsEnabled = isEditable;
                SaveChangesButton.IsEnabled = isEditable;
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, является ли выбранный элемент запросом на обслуживание
            if (RequestsListView.SelectedItem is ServiceRequest selectedRequest)
            {
                // Обновляем статус выбранного запроса
                selectedRequest.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                // Обновляем комментарии техника
                selectedRequest.TechnicianComments = TechnicianCommentsTextBox.Text;

                // Обновляем запрос в базе данных
                db.ServiceRequests.Update(selectedRequest);

                // Сохраняем изменения в базе данных
                db.SaveChanges();

                // Перезагружаем список запросов для отображения обновленных данных
                LoadServiceRequests();

                // Показываем сообщение об успешном сохранении
                MessageBox.Show("Changes saved successfully!");
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Навигация на страницу входа
            NavigationService.Navigate(new LoginPage());
        }
    }
}
