using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Api.Domain.Entities;

public class Product(Guid id, string description, double price)
{
    [Key]
    [Column("id")]
    public Guid Id { get; init; } = id;

    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [Column("description")]
    public string Description { get; private set; } = description;

    [Required]
    [Column("price")]
    public double Price { get; private set; } = price;

    public void SetDescription(string description) => Description = description;
    public void SetPrice(double price) => Price = price;
}
