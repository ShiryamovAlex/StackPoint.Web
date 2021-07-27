using System.ComponentModel.DataAnnotations;

namespace StackPoint.Data.Models
{
    /// <summary>
    /// Организация
    /// </summary>
    public class Organisation
    {
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
