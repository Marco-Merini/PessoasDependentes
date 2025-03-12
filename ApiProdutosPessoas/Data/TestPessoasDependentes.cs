using ApiPessoasDependentesTest.Data.Map;
using ApiPessoasDependentesTest.Models;
using ApiProdutosPessoas.Data.Map;
using ApiProdutosPessoas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Data
{
    public class TestPessoasDependentes : DbContext
    {
        public TestPessoasDependentes(DbContextOptions<TestPessoasDependentes> options)
            : base(options)
        {
        }

        public DbSet<PessoaModel> Pessoas { get; set; }
        public DbSet<DependenteModel> Dependentes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new DependenteMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
