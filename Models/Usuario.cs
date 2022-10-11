using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Models
{
    public class Usuario
    {
        public int Id {get; set;}
        public string Nome {get; set;}
        public string Email{get; set;}
        public string Sexo{get; set; }
        public string RG {get; set;}
        public string CPF {get; set;}
        public string NomeMae {get; set;}
        public string SituacaoCadastro{get; set;}
        public DateTimeOffset DataCadastro {get;set;} // formato indicado para banco de dados

        public Contato Contato {get; set;} // relacionamento com contato 1:1
        
        // um usuario pode ter varios endereços de entrega
        public ICollection<EnderecoEntrega> EnderecosEntrega {get; set;} // relacionamento 1:n
// um usuario pode ter uma lista de departamento e departamento pode ter uma lista de usuaios 
        public ICollection<Departamento> Departamentos {get; set;} // relacionamento n:n


    }
}