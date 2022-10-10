using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceAPI.Models;

namespace eCommerceAPI.Data.Repositories
{
    public interface IUsuarioRepository
    {

        // removemos o nome Usuario da frente dos metodos para ficar mais generico GetUsuario
        public List<Usuario> Get();
        public Usuario Get(int id);
        public void Insert(Usuario usuario);
        public void Update(Usuario usuario);
        public void Delete (int id);
    }
}