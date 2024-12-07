using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoSzerelo.Shared;

public class Ugyfel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(15)]
    public string Nev { get; set; }
    
    public string Lakcim { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}