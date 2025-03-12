using ApiPessoasDependentesTest.Models;
using ApiProdutosPessoas.Data;
using ApiProdutosPessoas.Models;
using ApiProdutosPessoas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Repositories
{
    public class DependenteRepositorio
    {
        private readonly TestPessoasDependentes _dbContext;

        public DependenteRepositorio(TestPessoasDependentes dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DependenteModel> VincularDependente(int codigoPessoa, int idResponsavel)
        {
            // Verifica se a pessoa (dependente) existe no banco de dados
            var pessoaDependente = await _dbContext.Pessoas.FirstOrDefaultAsync(p => p.Codigo == codigoPessoa);
            if (pessoaDependente == null)
            {
                throw new Exception("Pessoa (dependente) não encontrada. O código fornecido não existe no banco de dados.");
            }

            // Verifica se o responsável existe no banco de dados
            var pessoaResponsavel = await _dbContext.Pessoas.FirstOrDefaultAsync(p => p.Codigo == idResponsavel);
            if (pessoaResponsavel == null)
            {
                throw new Exception("Responsável não encontrado. O código fornecido não existe no banco de dados.");
            }

            // Gera um IDdependente único
            var idDependente = await CodeGenerator.GenerateUniqueDependenteCode(_dbContext);

            // Cria o dependente
            var dependente = new DependenteModel
            {
                IDdependente = idDependente,
                Codigo = codigoPessoa,
                IDresponsavel = idResponsavel // Usa o código da pessoa como IDresponsavel
            };

            // Adiciona o dependente ao banco de dados
            _dbContext.Dependentes.Add(dependente);
            await _dbContext.SaveChangesAsync();

            return dependente;
        }

        public async Task<bool> DesvincularDependente(int idDependente, int codigoPessoa)
        {
            var dependente = await _dbContext.Dependentes
                .FirstOrDefaultAsync(d => d.IDdependente == idDependente && d.Codigo == codigoPessoa);

            if (dependente == null)
            {
                throw new Exception("Dependente não encontrado.");
            }

            _dbContext.Dependentes.Remove(dependente);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}