using System.Data;
using Dapper;
using eCommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace eCommerceAPI.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private IDbConnection _contexto; 
        public UsuarioRepository()
        {
         _contexto = new SqlConnection(
          @"Server=localhost;
         Database=ApiDapper;
         Trusted_Connection=True;
         TrustServerCertificate=True;"
         );
        }
       
  
        public List<Usuario> Get()
        {
            List<Usuario> usuarios = new List<Usuario>();
            string sql = 
            @"SELECT U.*, C.*, EE.*, D.* 
            FROM Usuarios as U 
            LEFT JOIN Contatos C ON C.UsuarioId = U.Id 
            LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id 
            LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id 
            LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id";

            _contexto.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>( sql,  
                (usuario, contato, enderecoEntrega, departamento ) => {

                    //Validar usuario  para evitar duplicidade
                    if( usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        
                        usuarios.Add(usuario); 
                    } else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    //Verificar Endereço de Entrega.
                    if( usuario.EnderecosEntrega.SingleOrDefault(a=>a.Id == enderecoEntrega.Id) == null){
                        usuario.EnderecosEntrega.Add(enderecoEntrega);
                    }

                    //Verificar Departamento.
                    if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                    {
                        usuario.Departamentos.Add(departamento);
                    }

                    return usuario;
                });

             return usuarios;
        }

        public Usuario Get(int id)
        {
            List<Usuario> usuarios = new List<Usuario>();
            string sql =
             @"SELECT U.*, C.*, EE.*, D.* 
             FROM Usuarios as U 
             LEFT JOIN Contatos C ON C.UsuarioId = U.Id 
             LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id 
             LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id 
             LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id 
             WHERE U.Id = @Id";

            _contexto.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) => {

                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                    {
                        usuario.EnderecosEntrega.Add(enderecoEntrega); 
                    }

                    if (usuario.Departamentos.SingleOrDefault(a =>  a.Id == departamento.Id)  == null)
                    {
                        usuario.Departamentos.Add(departamento); 
                    }

                    return usuario;

                }, new { Id = id });

            return usuarios.SingleOrDefault();
        }

        public void Insert(Usuario usuario)
        {
            _contexto.Open();
            var transaction = _contexto.BeginTransaction();
            try
            {
                string sql = 
                @"INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) 
                VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); 
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
                usuario.Id = _contexto.Query<int>(sql, usuario, transaction).Single();

                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    string sqlContato = @"INSERT INTO Contatos(UsuarioId, Telefone, Celular) 
                    VALUES (@UsuarioId, @Telefone, @Celular); 
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    usuario.Contato.Id = _contexto.Query<int>( sqlContato, usuario.Contato, transaction).Single(); 
                }

                if(usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach(var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = 
                        @"INSERT INTO EnderecosEntrega(UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) 
                        VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); 
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        enderecoEntrega.Id = _contexto.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();
                    }
                }

                if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {
                        string sqlUsuariosDepartamentos = 
                        @"INSERT INTO UsuariosDepartamentos (UsuarioId, DepartamentoId)
                         VALUES (@UsuarioId, @DepartamentoId)"; 

                        _contexto.Execute(sqlUsuariosDepartamentos, new { UsuarioId = usuario.Id, DepartamentoId = departamento.Id }, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try { 
                    transaction.Rollback();
                }
                catch (Exception){}
            }
            finally
            {
                _contexto.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            _contexto.Open(); 
            var transaction = _contexto.BeginTransaction();

            try
            {
                string sql = 
                @"UPDATE Usuarios SET Nome = @Nome, 
                Email = @Email, 
                Sexo = @Sexo, 
                RG = @RG, 
                CPF = @CPF, 
                NomeMae = @NomeMae, 
                SituacaoCadastro = @SituacaoCadastro, 
                DataCadastro = @DataCadastro 
                WHERE Id = @Id";

                _contexto.Execute(sql, usuario, transaction);

                if(usuario.Contato != null) { 
                    string sqlContato = 
                    @"UPDATE Contatos SET 
                    UsuarioId = @UsuarioId, 
                    Telefone = @Telefone, 
                    Celular = @Celular 
                    WHERE Id = @Id";
                    _contexto.Execute(sqlContato, usuario.Contato, transaction);
                }

                string sqlDeletarEnderecosEntrega = 
                @"DELETE FROM EnderecosEntrega 
                WHERE UsuarioId = @Id";

                _contexto.Execute(sqlDeletarEnderecosEntrega, usuario, transaction);

                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = 
                        @"INSERT INTO EnderecosEntrega(UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) 
                        VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); 
                        SELECT CAST(SCOPE_IDENTITY() 
                        AS INT);";

                        enderecoEntrega.Id = _contexto.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();
                    }
                }

                string sqlDeletarUsuariosDapartamentos = "DELETE FROM UsuariosDepartamentos WHERE UsuarioId = @Id";
                _contexto.Execute(sqlDeletarUsuariosDapartamentos, usuario, transaction);

                if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {
                        string sqlUsuariosDepartamentos = 
                        @"INSERT INTO UsuariosDepartamentos (UsuarioId, DepartamentoId)
                         VALUES (@UsuarioId, @DepartamentoId)";

                        _contexto.Execute(sqlUsuariosDepartamentos, new { UsuarioId = usuario.Id, DepartamentoId = departamento.Id }, transaction);
                    }
                }

                transaction.Commit();
            } catch(Exception) {
                try { 
                    transaction.Rollback();
                }catch(Exception)
                {
                }
            } finally {
                _contexto.Close();
            }            
        }

        public void Delete(int id)
        {
            _contexto.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
        }

    }


}

 