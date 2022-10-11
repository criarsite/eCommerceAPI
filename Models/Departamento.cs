using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Models
{
    public class Departamento
    {
        public int Id {get; set;}
        public string Nome {get; set;}
        // um departamento pode ter uma lista de usuaios e usuario pode ter uma lista de departamento 
        public ICollection<Usuario> Usuarios {get; set;} // relacionamento n:n
    }
}