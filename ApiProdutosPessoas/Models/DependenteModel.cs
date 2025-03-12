using ApiProdutosPessoas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiPessoasDependentesTest.Models
{
    public class DependenteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDdependente { get; set; }

        [Required]
        public int Codigo { get; set; }

        [Required]
        public int IDresponsavel { get; set; }

        [ForeignKey("Codigo")]
        [JsonIgnore]
        public PessoaModel Pessoa { get; set; }
    }
}
