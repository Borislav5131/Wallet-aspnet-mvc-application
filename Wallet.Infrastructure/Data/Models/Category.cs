using System.ComponentModel.DataAnnotations;
using static Wallet.Infrastructure.Data.DataConstants;

namespace Wallet.Data.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(CategoryMaxNameLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CategoryDescriptionMaxLenght)]
        public string Description { get; set; }

        public List<Asset> Assets { get; set; } = new List<Asset>();
    }
}
