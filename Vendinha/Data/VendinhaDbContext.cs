using Microsoft.EntityFrameworkCore;
using Vendinha.Models;

namespace Vendinha.Data
{
    public class VendinhaDbContext : DbContext
    {
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Divida> Dividas => Set<Divida>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                Environment.GetEnvironmentVariable(
                    "ConnectionStrings__Default"
                )
            );

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            var modelCliente = modelBuilder.Entity<Cliente>();

            modelCliente.ToTable("clientes");

            modelCliente.HasKey(c => c.Id);

            modelCliente.Property(c => c.Id)
                .HasColumnName("id");

            modelCliente.Property(c => c.nome_completo)
                .HasColumnName("nome_completo")
                .IsRequired();

            modelCliente.Property(c => c.cpf)
                .HasColumnName("cpf")
                .IsRequired();

            modelCliente.Property(c => c.data_nascimento)
                .HasColumnName("data_nascimento")
                .IsRequired();

            modelCliente.Property(c => c.Email)
                .HasColumnName("email");

            modelCliente.HasIndex(c => c.cpf)
                .IsUnique();

            var modelDivida = modelBuilder.Entity<Divida>();

            modelDivida.ToTable("dividas");

            modelDivida.HasKey(d => d.Id);

            modelDivida.Property(d => d.Id)
                .HasColumnName("id");

            modelDivida.Property(d => d.Valor)
                .HasColumnName("valor")
                .HasPrecision(10, 2);

            modelDivida.Property(d => d.Paga)
                .HasColumnName("paga");

            modelDivida.Property(d => d.data_criacao)
                .HasColumnName("data_criacao");

            modelDivida.Property(d => d.data_pagamento)
                .HasColumnName("data_pagamento");

            modelDivida.Property(d => d.ClienteId)
                .HasColumnName("cliente_id");

            modelDivida.HasOne(d => d.Cliente)
                .WithMany(c => c.Dividas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }


}
