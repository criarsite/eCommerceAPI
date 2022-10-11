using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Models
{
    public class EnderecoEntrega
    {
        public int Id {get; set;}
        public int UsuarioId {get; set;}
         public string NomeEndereco {get; set;}
        public string CEP {get; set;}
        public string Estado {get; set;}
        public string Cidade {get; set;}
        public string Endereco {get; set;}
        public string Numero {get; set;}
        public string Complemento {get; set;}

        // um endereço perternce a um usuario
        public Usuario Usuario {get; set;} // eu pego o endereço e vejo a qual usuario ele pertence 1:1
   
   
   
   
    }
}