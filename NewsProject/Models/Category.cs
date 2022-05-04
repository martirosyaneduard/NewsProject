using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsProject.Models
{
    [Index("Name", IsUnique = true, Name = "Name_Index")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }=string.Empty;
        [JsonIgnore]
        public List<New> News { get; set; } = new();
    }
}
