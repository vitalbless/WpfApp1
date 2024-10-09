using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }  // Первичный ключ. Уникальный идентификатор для каждого пользователя.

        [Required]
        public string Username { get; set; }  // Имя пользователя. Уникальное поле для аутентификации.

        [Required]
        public string Password { get; set; }  // Пароль пользователя. Хранится в зашифрованном виде.

        [ForeignKey("Role")]
        public int RoleId { get; set; }  // Внешний ключ, связывающий пользователя с ролью.

        public Role Role { get; set; }  // Навигационное свойство для ссылки на роль пользователя.
    }
}
