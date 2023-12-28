using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kit19.SearchProduct.App.Models;

public class ProductSearchViewModel
{
    public string? ProductName { get; set; }
    public string? Size { get; set; }
    public decimal? Price { get; set; }
    public DateTime? MfgDate { get; set; }
    public string? Category { get; set; }
    public string? Conjunction { get; set; }
    public List<Product> SearchResults { get; set; }

    public ProductSearchViewModel()
    {
        SearchResults = new List<Product>();
    }
}

[Table("tbl_Product")]
public class Product
{
    [Key] // Indicates that this property is the primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
    public int ProductId { get; set; }

    [Required] // This field is required
    [StringLength(255)] // Maximum string length
    public string? ProductName { get; set; }

    [StringLength(50)] // Maximum string length
    public string? Size { get; set; }

    [Column(TypeName = "decimal(18, 2)")] // Specifies the precision and scale for the decimal
    public decimal? Price { get; set; }

    [DataType(DataType.Date)] // Specifies the type of data
    public DateTime? MfgDate { get; set; }

    [StringLength(50)] // Maximum string length
    public string? Category { get; set; }
}