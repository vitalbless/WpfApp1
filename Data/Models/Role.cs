using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfApp1.Data.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }  // Первичный ключ. Уникальный идентификатор для каждой роли.

        [Required]
        public string RoleName { get; set; }  // Название роли (например, "Operator", "Technician", "Administrator").

        public ICollection<User> Users { get; set; }  // Навигационное свойство для ссылки на всех пользователей, имеющих эту роль.
    }
}
