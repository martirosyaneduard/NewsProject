using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsProject.Models
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }=string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }= string.Empty;

        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }
        public List<Category> Categories { get; set; } = new();
    }
}
