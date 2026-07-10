using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Data
{
    public class EstoqueLiaTattooContext : DbContext
    {
        public EstoqueLiaTattooContext(DbContextOptions<EstoqueLiaTattooContext> options)
            : base(options)
        {
        }

        public DbSet<Movimentacao> Movimentacao { get; set; } = default!;
        public DbSet<Material> Material { get; set; } = default!;
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Tinta> Tinta { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Agulhas" },
                new Categoria { Id = 2, Nome = "Tintas" },
                new Categoria { Id = 3, Nome = "Descartaveis" },
                new Categoria { Id = 4, Nome = "Equipamentos" }
            );

            modelBuilder.Entity<Material>().HasData(
                new Material
                {
                    Id = 1,
                    Nome = "Agulha 3RL Cartucho",
                    CategoriaId = 1,
                    QuantidadeAtual = 25,
                    QuantidadeMinima = 5,
                    PrecoUnitario = 8.50m
                },
                new Material
                {
                    Id = 2,
                    Nome = "Agulha 7MG Cartucho",
                    CategoriaId = 1,
                    QuantidadeAtual = 12,
                    QuantidadeMinima = 5,
                    PrecoUnitario = 8.50m
                },
                new Material
                {
                    Id = 3,
                    Nome = "Tinta Preta Dynamic 240ml",
                    CategoriaId = 2,
                    QuantidadeAtual = 3,
                    QuantidadeMinima = 1,
                    PrecoUnitario = 190.00m
                },
                new Material
                {
                    Id = 4,
                    Nome = "Tinta Vermelha 140ml",
                    CategoriaId = 2,
                    QuantidadeAtual = 2,
                    QuantidadeMinima = 1,
                    PrecoUnitario = 120.00m
                },
                new Material
                {
                    Id = 5,
                    Nome = "Batoque Descartavel P",
                    CategoriaId = 3,
                    QuantidadeAtual = 500,
                    QuantidadeMinima = 50,
                    PrecoUnitario = 0.05m
                },
                new Material
                {
                    Id = 6,
                    Nome = "Plastico Filme Rolo",
                    CategoriaId = 3,
                    QuantidadeAtual = 2,
                    QuantidadeMinima = 2,
                    PrecoUnitario = 15.00m
                }
            );
        }
    }
}
