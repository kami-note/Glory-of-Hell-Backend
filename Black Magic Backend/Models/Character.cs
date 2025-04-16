using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMagicBackend.DataTypes;

namespace Black_Magic_Backend.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = new User();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Vector3 Position { get; set; } = new(0, 0, 0);
    }
}
