using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Data.Models
{
    public class ServiceRequest
    {
        [Key]
        public int RequestId { get; set; }  // Первичный ключ. Уникальный идентификатор для каждой заявки.

        [Required]
        public string Description { get; set; }  // Описание проблемы или услуги, требующей выполнения.

        public string Status { get; set; }  // Статус заявки (например, "Pending", "In Progress", "Completed").

        public string TechnicianComments { get; set; } = string.Empty;   // Комментарии техника по заявке.

        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; }  // Внешний ключ, указывающий на пользователя, создавшего заявку.

        public User CreatedBy { get; set; }  // Навигационное свойство для ссылки на пользователя, создавшего заявку.

        [ForeignKey("AssignedTo")]
        public int? AssignedToId { get; set; }  // Внешний ключ, указывающий на техника, которому назначена заявка. Может быть null, если заявка еще не назначена.

        public User AssignedTo { get; set; }  // Навигационное свойство для ссылки на техника, назначенного для выполнения заявки.

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Дата и время создания заявки. По умолчанию устанавливается текущее время.
    }
}
