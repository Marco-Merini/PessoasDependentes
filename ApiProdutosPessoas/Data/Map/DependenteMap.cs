using ApiPessoasDependentesTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPessoasDependentesTest.Data.Map
{
    public class DependenteMap : IEntityTypeConfiguration<DependenteModel>
    {
        public void Configure(EntityTypeBuilder<DependenteModel> builder)
        {
            builder.HasKey(x => x.IDdependente);
            builder.Property(x => x.Codigo).IsRequired();

            builder.HasOne(x => x.Pessoa)
                  .WithMany()
                  .HasForeignKey(x => x.Codigo);
        }
    }
}
