using System.ComponentModel.DataAnnotations;
using static Wallet.Infrastructure.Data.DataConstants.Category;

namespace Wallet.Core.ViewModels.Category
{
    public class EditCategoryModel
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(CategoryMaxNameLenght, ErrorMessage = "Name must be max 30 characters!"), MinLength(CategoryMinNameLenght, ErrorMessage = "Name must be min 3 characters!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(CategoryDescriptionMaxLenght, ErrorMessage = "Category must be max 500 characters!"), MinLength(CategoryDescriptionMinLenght, ErrorMessage = "Name must be min 10 characters!")]
        public string? Description { get; set; }
    }
}
