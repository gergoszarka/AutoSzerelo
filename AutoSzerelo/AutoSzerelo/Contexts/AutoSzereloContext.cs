using AutoSzerelo.Shared;
using Microsoft.EntityFrameworkCore;

namespace AutoSzerelo.Contexts;

public class AutoSzereloContext : DbContext
{
    public AutoSzereloContext(DbContextOptions<AutoSzereloContext> options)
        : base(options)
    {
        
    }
    
    public virtual DbSet<Ugyfel> Ugyfelek { get; set; }
    
    public virtual DbSet<Munka> Munkak { get; set; }
}