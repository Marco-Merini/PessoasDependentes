using ApiPessoasDependentesTest.Models;
using ApiProdutosPessoas.Models;
using ApiProdutosPessoas.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenteController : ControllerBase
    {
        private readonly DependenteRepositorio _dependenteRepositorio;

        public DependenteController(DependenteRepositorio dependenteRepositorio)
        {
            _dependenteRepositorio = dependenteRepositorio;
        }

        [HttpPost("vincular")]
        public async Task<ActionResult<DependenteModel>> VincularDependente([FromBody] DependenteModel dependenteModel)
        {
            try
            {
                // Verifica se os campos "CodigoPessoa" e "IDresponsavel" foram preenchidos
                if (dependenteModel.Codigo == 0 || dependenteModel.IDresponsavel == 0)
                {
                    return BadRequest("Os campos 'CodigoPessoa' e 'IDresponsavel' são obrigatórios.");
                }

                // Vincula o dependente
                var dependente = await _dependenteRepositorio.VincularDependente(dependenteModel.Codigo, dependenteModel.IDresponsavel);
                return Ok(dependente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("desvincular")]
        public async Task<ActionResult<bool>> DesvincularDependente(int idDependente, int codigoPessoa)
        {
            try
            {
                bool deletado = await _dependenteRepositorio.DesvincularDependente(idDependente, codigoPessoa);
                return Ok(deletado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}