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
    public class PessoaRepositorio : InterfacePessoa
    {
        private readonly TestPessoasDependentes _dbContext;

        public PessoaRepositorio(TestPessoasDependentes dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PessoaModel>> BuscarTodasPessoas()
        {
            return await _dbContext.Pessoas
                .ToListAsync();
        }

        public async Task<List<PessoaModel>> BuscarPessoasPorNome(string nome)
        {
            return await _dbContext.Pessoas
                .Where(p => p.Nome.Contains(nome))
                .ToListAsync();
        }

        public async Task<PessoaModel> BuscarPessoaPorCodigo(int codigo)
        {
            var pessoa = await _dbContext.Pessoas
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
            return pessoa;
        }

        public async Task<PessoaModel> AdicionarPessoa(PessoaModel pessoa)
        {
            // Gera um código único para a pessoa se for 0
            if (pessoa.Codigo == 0)
            {
                pessoa.Codigo = await CodeGenerator.GenerateUniquePessoaCode(_dbContext);
            }

            // Gera um CPF aleatório único se for nulo, vazio ou "string"
            if (string.IsNullOrEmpty(pessoa.CPF) || pessoa.CPF == "string")
            {
                pessoa.CPF = await CodeGenerator.GenerateUniqueCPF(_dbContext);
            }

            // Gera um CEP aleatório se for nulo, vazio ou "string"
            if (string.IsNullOrEmpty(pessoa.CEP) || pessoa.CEP == "string")
            {
                pessoa.CEP = CodeGenerator.GenerateRandomCEP();
            }

            await _dbContext.Pessoas.AddAsync(pessoa);
            await _dbContext.SaveChangesAsync();

            return pessoa;
        }

        public async Task<PessoaModel> AtualizarPessoa(PessoaModel pessoa, int codigo)
        {
            var pessoaExistente = await _dbContext.Pessoas.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (pessoaExistente == null)
            {
                throw new Exception($"Pessoa com o código {codigo} não foi encontrada.");
            }

            pessoaExistente.Nome = pessoa.Nome;
            pessoaExistente.Idade = pessoa.Idade;

            // Atualiza o CPF apenas se não for nulo, vazio ou "string"
            if (!string.IsNullOrEmpty(pessoa.CPF) && pessoa.CPF != "string")
            {
                pessoaExistente.CPF = pessoa.CPF;
            }

            pessoaExistente.Logradouro = pessoa.Logradouro;
            pessoaExistente.NumeroEstabelecimento = pessoa.NumeroEstabelecimento;
            pessoaExistente.Bairro = pessoa.Bairro;

            // Atualiza o CEP apenas se não for nulo, vazio ou "string"
            if (!string.IsNullOrEmpty(pessoa.CEP) && pessoa.CEP != "string")
            {
                pessoaExistente.CEP = pessoa.CEP;
            }

            _dbContext.Pessoas.Update(pessoaExistente);
            await _dbContext.SaveChangesAsync();

            return pessoaExistente;
        }

        public async Task<bool> DeletarPessoa(int codigo)
        {
            var pessoa = await _dbContext.Pessoas.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (pessoa == null)
            {
                throw new Exception($"Pessoa com o código {codigo} não foi encontrada.");
            }

            _dbContext.Pessoas.Remove(pessoa);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}