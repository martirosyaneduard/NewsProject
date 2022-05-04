using NewsProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsProject.DataTransferObject
{
    public class NewDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Column(TypeName = "Date")]
        [Range(typeof(DateTime), "01-01-1970", "01-05-2022")]
        public DateTime CreatedAt { get; set; }
        public List<int> CategoriesId { get; set; } = new();
    }
}
