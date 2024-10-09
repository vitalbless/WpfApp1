using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data.Models;
using System.Linq;

namespace WpfApp1.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }  // Таблица пользователей.
        public DbSet<Role> Roles { get; set; }  // Таблица ролей.
        public DbSet<ServiceRequest> ServiceRequests { get; set; }  // Таблица заявок на обслуживание.

        public ApplicationContext()
        {
            Database.EnsureCreated();  // Гарантирует, что база данных будет создана, если она не существует.
            SeedRoles();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectingString());  // Конфигурирует контекст для использования SQL Server с заданной строкой подключения.
        }
        private void SeedRoles()
        {
            if (!Roles.Any())
            {
                Roles.AddRange(
                    new Role { RoleName = "Оператор" },
                    new Role { RoleName = "Техник" },
                    new Role { RoleName = "Администратор" }
                );
                SaveChanges();
            }
        }

        private string GetConnectingString()
        {
            return new SqlConnectionStringBuilder()
            {
                DataSource = "localhost",  // Адрес сервера базы данных.
                IntegratedSecurity = true,  // Использование аутентификации Windows.
                InitialCatalog = "Project7"  // Имя базы данных.
            }.ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);  // Конфигурирует связь "один ко многим" между Role и User.
        }
    }
}
