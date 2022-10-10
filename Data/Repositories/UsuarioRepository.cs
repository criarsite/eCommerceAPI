using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceAPI.Models;

namespace eCommerceAPI.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
       private static List<Usuario> _db = new List<Usuario>()
       {
            // Simulando os Usuarios que teria no banco de dados
        new Usuario(){ Id=1, Nome="Thais Melo", Email="thazitta@gmail.com"},
        new Usuario() {Id=2 , Nome="Luana Gomes", Email="lolo@gmail.com"},
        new Usuario() {Id=3 , Nome="Alice Gomes", Email="lili@gmail.com"},
        new Usuario() {Id=4, Nome="Washington Gomes", Email="oktuga@gmail.com"}
       };

        public List<Usuario> Get()
        {

            /*Para retornar todos os usuarios apenas retorne o 
            e muito simples apenas retorne o banco de dados _db*/
            return _db;
        }

        public Usuario Get(int id)
        {
            /*  para pegar apenas um usuario eu tenho que retonar o banco de dados 
           pesquisando este usuario onde faz a pesquisa baseada no ID _db.FirstOrDefault( a => a.Id == id); 
           */
            return _db.FirstOrDefault(a => a.Id == id);
        }

        public void Insert(Usuario usuario)
        {
          // logica para encontra  o ultimo usuario e se nao encontrar retornar nulo
          var ultimoUsuario = _db.LastOrDefault();
          if (ultimoUsuario == null)
          {
            usuario.Id =1;
          } else{
            usuario.Id = ultimoUsuario.Id;
            usuario.Id++;
          }

         // para inserir usamos o .Add (usuario)
         _db.Add(usuario);

        }

        public void Update(Usuario usuario)
        {
           // para atualizar precisamos achar o registro _db.FirstOrDefault(a => a.Id == id); re remover e depois adiconar outro
         
          _db.Remove( _db.FirstOrDefault(a => a.Id == usuario.Id));
          _db.Add(usuario);
        }

         public void Delete(int id)
        {
           _db.Remove( _db.FirstOrDefault(a => a.Id == id));
        }
    }
}