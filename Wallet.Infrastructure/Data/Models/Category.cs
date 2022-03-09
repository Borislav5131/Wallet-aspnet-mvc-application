using System.ComponentModel.DataAnnotations;
using static Wallet.Data.DataConstants;

namespace Wallet.Data.Models
{
    public class Category
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(GategoryMaxNameLenght)]
        public string Name { get; set; }

        public IEnumerable<Asset> Assets { get; set; } = new List<Asset>();
    }
}
