using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using eCommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace eCommerceAPI.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private IDbConnection _connection; 
        public UsuarioRepository() // conexao com o banco de dados
        {
         _connection = new SqlConnection("Server=localhost;Database=ApiDapper;Trusted_Connection=True;TrustServerCertificate=True;");
        }
       

        public List<Usuario> Get()
        {

            /*Para retornar todos os usuarios apenas retorne o 
            e muito simples apenas retorne o banco de dados _db*/
            //return _db;
            return _connection.Query<Usuario>("SELECT * FROM Usuarios").ToList();
        }

        public Usuario Get(int id)
        {
            /*  para pegar apenas um usuario eu tenho que retonar o banco de dados 
           pesquisando este usuario onde faz a pesquisa baseada no ID _db.FirstOrDefault( a => a.Id == id); 
           */
            //return _db.FirstOrDefault(a => a.Id == id);
            return _connection.QuerySingleOrDefault<Usuario>("SELECT * FROM Usuarios WHERE Id =@Id", new {Id = id});
        }

        public void Insert(Usuario usuario)
        {
         /* // logica para encontra  o ultimo usuario e se nao encontrar retornar nulo
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
        */
       string sql ="INSERT INTO [dbo].[Usuarios] (Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); SELECT CAST(SCOPE_IDENTITY() AS INT);";

        _connection.Query<int>(sql, usuario ).Single();

        }

        public void Update(Usuario usuario)
        {
           /* para atualizar precisamos achar o registro _db.FirstOrDefault(a => a.Id == id); re remover e depois adiconar outro
          _db.Remove( _db.FirstOrDefault(a => a.Id == usuario.Id));
          _db.Add(usuario);
          */
// Codigo SQL de atualizacao
          string sql = "UPDATE [dbo].[Usuarios] SET Nome = @nome, Email = @email, Sexo = @sexo, RG = @rg, CPF = @cpf, NomeMae = @nomeMae, SituacaoCadastro = @situacaoCadastro, DataCadastro = @dataCadastro WHERE Id = @id";
             // nao vamos usar o Query pois usamos ele quando queremos algum tipo de retorno 
             // quando quremos executar apenas um comando que nao vai ter retorno usamos o meto .Execute()

             _connection.Execute(sql, usuario);
             //passa o conexao com o bando de dados com o metodo execute passando como paramento o comando sql e o objeto de retorno.
        }

         public void Delete(int id)
        {
                //_db.Remove( _db.FirstOrDefault(a => a.Id == id));
           _connection.Execute("DELETE FROM [dbo].[Usuarios] WHERE Id = @id", new {Id = id});

        }





             /* este medodo estava no topo e veio para o final 
           private static List<Usuario> _db = new List<Usuario>()
       {
            // Simulando os Usuarios que teria no banco de dados
        new Usuario(){ Id=1, Nome="Thais Melo", Email="thazitta@gmail.com"},
        new Usuario() {Id=2 , Nome="Luana Gomes", Email="lolo@gmail.com"},
        new Usuario() {Id=3 , Nome="Alice Gomes", Email="lili@gmail.com"},
        new Usuario() {Id=4, Nome="Washington Gomes", Email="oktuga@gmail.com"}
       };
               */


        
    }
}