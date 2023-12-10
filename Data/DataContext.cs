using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LibApi;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString =
                    "Server=localhost;User Id=root;Password=password;Database=LibraryApp";
                optionsBuilder.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public DbSet<Livro> Livro { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Reserva> Reserva { get; set; }

        public DbSet<Emprestimo> Emprestimo { get; set; }


    }
}
