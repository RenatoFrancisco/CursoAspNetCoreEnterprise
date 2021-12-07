using Core.Data;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public DbSet<Produto> Produtos { get; set; }
        
        public CatalogoContext(DbContextOptions<CatalogoContext> options) :  base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(200)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }

        public async Task<bool> CommitAsync() => await SaveChangesAsync() > 0;
    }
}