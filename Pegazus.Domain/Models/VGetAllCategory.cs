using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pegazus.Domain.Models
{
    [Keyless]
    public partial class VGetAllCategory
    {
        [Required]
        [StringLength(50)]
        public string ParentProductCategoryName { get; set; }
        [StringLength(50)]
        public string ProductCategoryName { get; set; }
        [Column("ProductCategoryID")]
        public int? ProductCategoryId { get; set; }
    }
}
