using System.ComponentModel.DataAnnotations;

namespace CrudStudents.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; } = string.Empty;

        [Range(18, 120, ErrorMessage = "Edad debe estar entre 18 y 120")]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}