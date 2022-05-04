using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsProject.DataTransferObject
{
    public class CategoryDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public List<int> News { get; set; } = new();
    }
}
