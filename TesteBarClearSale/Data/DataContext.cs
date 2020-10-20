using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBarClearSale.Models;
using Microsoft.EntityFrameworkCore;
namespace TesteBarClearSale.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
            :base(options)
        {             
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Comanda> Comandas { get; set; }
        public DbSet<ItemComanda> ItensComanda { get; set; }
        public DbSet<Promocao> Promocoes { get; set; }        
        public DbSet<PromocaoRequisito> PromocaoRequisitos { get; set; }
        public DbSet<ComandaPromocao> ComandaPromocoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
       

    }
}
