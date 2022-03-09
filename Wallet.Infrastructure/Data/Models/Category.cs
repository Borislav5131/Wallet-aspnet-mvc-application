using System.ComponentModel.DataAnnotations;
using static Wallet.Data.DataConstants;

namespace Wallet.Data.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(GategoryMaxNameLenght)]
        public string Name { get; set; }

        [MaxLength(CategoryDescriptionMaxLenght)]
        public string? Description { get; set; }

        public List<Asset> Assets { get; set; } = new List<Asset>();
    }
}
