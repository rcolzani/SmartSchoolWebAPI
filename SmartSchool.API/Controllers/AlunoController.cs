using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
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
    private readonly IRepository _repo;
    public AlunoController(IRepository repo)
    {
      _repo = repo;
    }

    // GET: api/<AlunoController>
    [HttpGet]
    public IActionResult Get()
    {
      return Ok(_repo.GetAllAlunos(true));
    }

    //Como existem duas rotas get iguais é necessário especificar o parâmetro de uma delas. Por padrão todas são string, então é necessário especificar a int
    // GET api/<AlunoController>/5
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
      var aluno = _repo.GetAlunoById(id);
      if (aluno == null) return BadRequest("Aluno não encontrado");
      return Ok(aluno);
    }

    //O nome sem as chaves faz com que sejam recebidos via queryparams. Não é obrigatório passar todo parametros, mas precisa validar se foram passados para não dar exception no código, porque podem vir vazios
    // GET api/<AlunoController>/filter?name=Ricardo&lastname=Colzani
    [HttpGet("filter")]
    public IActionResult GetByName(string name, string lastname)
    {
      if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(lastname))
      {
        return BadRequest("Parâmetros incorretos");
      }

      var alunos = _repo.GetAlunoByName(name, lastname);

      if (alunos.Count() <= 0) return BadRequest("Aluno não encontrado");
      return Ok(alunos);
    }

    // POST api/<AlunoController>
    [HttpPost]
    public IActionResult Post(Aluno aluno)
    {
      _repo.Add(aluno);
      _repo.SaveChanges();
      return Ok(aluno);
    }

    // PUT api/<AlunoController>/5
    [HttpPut()]
    public IActionResult Put(Aluno aluno)
    {
      var alunoExistente = _repo.GetAlunoById(aluno.Id);
      if (alunoExistente == null) return BadRequest("Aluno not found");

      _repo.Update(aluno);
      _repo.SaveChanges();
      return Ok(aluno);
    }

    // DELETE api/<AlunoController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var alunoExistente = _repo.GetAlunoById(id);
      if (alunoExistente == null) return BadRequest("Aluno not found");

      _repo.Remove(alunoExistente);
      _repo.SaveChanges();
      return Ok(_repo.GetAllAlunos(true));
    }
  }
}
