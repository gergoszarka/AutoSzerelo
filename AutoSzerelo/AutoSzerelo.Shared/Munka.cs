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
    [Range(typeof(int), "1900", "2024")]
    public int GyartasiEv { get; set; }
    
    public string MunkaKategoria { get; set; }
    
    public string Leiras { get; set; }
    
    [Required]
    [RegularExpression(@"^(10|[1-9])$")]
    public int HibaSuly { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{1}$")]
    public int MunkaAllapota { get; set; }
    
    public float MunkaOra { get; set; }
    
    

    
}