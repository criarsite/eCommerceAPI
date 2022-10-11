using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Models
{
    public class Contato
    {
        public int Id {get; set;}
        public int UsuarioId {get; set;}
        public string Telefone {get; set;}
        public string Celular{get; set;}
        public Usuario Usuario {get; set;} // um contato pertence a um usuario 1:1
    }
}