using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eCommerceAPI.Data.Repositories;
using eCommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace eCommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // cria um propriedade privada para representar o repositorio que vai conectar com o banco de dados
     private IUsuarioRepository _repository;
       // ja começa criando o construtor ctor e tab tab 
       public UsuariosController()
       {
        _repository = new UsuarioRepository();
       }

       /* CRUD CREATE - READ - UPDATE - DELETE
          GET    -> Obter a lista de usuarios
          GET    -> Obter o usuario passando o ID
          POST   -> Cadastrar um usuario
          PUT    -> Atualizar um usuario
          Delete -> Remover um usuario

          Estes sao os metodos HTTP que vamos estar utilizando para criar nossa API aqui 
          no nosso projeto
       */

        [HttpGet]
        //GET    -> Obter a lista de usuarios
        public IActionResult Get()
        {
            return Ok(_repository.Get()); //HTTP - 200 - OK
        }

        // GET    -> Obter o usuario passando o ID
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // cria uma variavel que recebe o usuario id 
            var usuario = _repository.Get(id);

            // verifica se o usuario existe ou nao
            if (usuario == null) // o usuario é igual a nulo?
            {
                
                return NotFound(); // HTTP - 404 - Nao encontrado
            }
               
               // se o usuario foi encontrato retorna ele 
            return Ok(usuario); // no corpo da pagina que vai em formato JSOM
        
          }


         [HttpPost]
        public IActionResult Insert([FromBody]Usuario usuario)
        {
         _repository.Insert(usuario);
         return Ok(usuario);
        }

        [HttpPatch]
       public IActionResult Update([FromBody] Usuario usuario)
       {
          _repository.Update(usuario);
          return Ok(usuario);
       }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
       {

        _repository.Delete(id);
        return Ok();
       }
        
    }
}