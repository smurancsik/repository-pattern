using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pegazus.Domain.Models
{
    [Table("ProductDescription", Schema = "SalesLT")]
    [Index(nameof(Rowguid), Name = "AK_ProductDescription_rowguid", IsUnique = true)]
    public partial class ProductDescription
    {
        public ProductDescription()
        {
            ProductModelProductDescriptions = new HashSet<ProductModelProductDescription>();
        }

        [Key]
        [Column("ProductDescriptionID")]
        public int ProductDescriptionId { get; set; }
        [Required]
        [StringLength(400)]
        public string Description { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [InverseProperty(nameof(ProductModelProductDescription.ProductDescription))]
        public virtual ICollection<ProductModelProductDescription> ProductModelProductDescriptions { get; set; }
    }
}
