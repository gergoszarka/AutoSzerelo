using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoSzerelo.Shared;

public class Munka
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public int UgyfelSzam { get; set; }
    
    [Required]
    [RegularExpression(@"^[A-Z]{3}\d{3}$")]
    public string Rendszam { get; set; }
    
    [Required]
    [Range(typeof(DateOnly), "1990-01-01", "2020-12-31")]
    public DateOnly GyartasiEv { get; set; }
    
    public string MunkaKategoria { get; set; }
    
    public string Leiras { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{1}$")]
    public int HibaSuly { get; set; }
    
    public int MunkaAllapota { get; set; }
    
    

    
}