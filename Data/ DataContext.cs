
using Microsoft.EntityFrameworkCore;
using QrAmparoApi.Models;

namespace QrAmparoApi.Data
{
    public class  DataContext : DbContext
    {
          public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<ResponsavelQr> ResponsaveisQr {get; set;}
        public DbSet<Idoso> Idosos {get; set;}
        public DbSet<Usuario> Usuarios { get; set; }
        
    }
}