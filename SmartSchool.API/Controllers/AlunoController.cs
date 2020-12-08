using Microsoft.AspNetCore.Mvc;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        public List<Aluno> Alunos = new List<Aluno>() { new Aluno { Nome = "Ricardo", Id = 1, Sobrenome = "Colzani", Telefone = "455645454654" },
        new Aluno {  Nome = "Raissa", Id = 2, Sobrenome = "Telles", Telefone = "5446454" } };

        // GET: api/<AlunoController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Alunos);
        }


        //Como existem duas rotas get iguais é necessário especificar o parâmetro de uma delas. Por padrão todas são string, então é necessário especificar a int
        // GET api/<AlunoController>/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null) return BadRequest("Aluno não encontrado");
            return Ok(aluno);
        }

        // GET api/<AlunoController>/nome
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Nome == name);
            if (aluno == null) return BadRequest("Aluno não encontrado");
            return Ok(aluno);
        }

        //O nome sem as chaves faz com que sejam recebidos via queryparams. Não é obrigatório passar todo parametros, mas precisa validar se foram passados para não dar exception no código, porque podem vir vazios
        // GET api/<AlunoController>/filter?name=Ricardo&lastname=Colzani
        [HttpGet("filter")]
        public IActionResult GetById(string name, string lastname)
        {
            Aluno aluno;
            name = name.ToLower();
            lastname = lastname.ToLower();

            if (string.IsNullOrEmpty(name) == false && string.IsNullOrEmpty(lastname)==false)
            {
                aluno = Alunos.FirstOrDefault(a => a.Nome.ToLower() == name && a.Sobrenome.ToLower() == lastname);
            }else if (string.IsNullOrEmpty(name) == false)
            {
                aluno = Alunos.FirstOrDefault(a => a.Nome.ToLower() == name);
            }else if (string.IsNullOrEmpty(lastname) == false)
            {
                aluno = Alunos.FirstOrDefault(a => a.Sobrenome.ToLower() == lastname);
            }
            else
            {
                return BadRequest("Parâmetros incorretos");
            }
            if (aluno == null) return BadRequest("Aluno não encontrado");
            return Ok(aluno);
        }

        // POST api/<AlunoController>
        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            int ultimoCodigo = Alunos.OrderBy(a => a.Id).LastOrDefault().Id;

            aluno.Id = ultimoCodigo+1;
            Alunos.Add(aluno);
            return Ok(Alunos);
        }

        // PUT api/<AlunoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok("deu boas");
        }

        // DELETE api/<AlunoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("deu boas");
        }
    }
}
