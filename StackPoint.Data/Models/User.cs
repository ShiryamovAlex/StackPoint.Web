using System.ComponentModel.DataAnnotations;

namespace StackPoint.Data.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        public Organisation Organisation { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        public long? OrganisationId { get; set; }
    }
}