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
    private readonly SmartContext _context;

    public AlunoController(SmartContext context)
    {
      this._context = context;
    }

    // GET: api/<AlunoController>
    [HttpGet]
    public IActionResult Get()
    {
      return Ok(_context.Alunos);
    }

    //Como existem duas rotas get iguais é necessário especificar o parâmetro de uma delas. Por padrão todas são string, então é necessário especificar a int
    // GET api/<AlunoController>/5
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
      var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
      if (aluno == null) return BadRequest("Aluno não encontrado");
      return Ok(aluno);
    }

    // GET api/<AlunoController>/nome
    [HttpGet("{name}")]
    public IActionResult Get(string name)
    {
      var aluno = _context.Alunos.FirstOrDefault(a => a.Nome == name);
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

      if (string.IsNullOrEmpty(name) == false && string.IsNullOrEmpty(lastname) == false)
      {
        aluno = _context.Alunos.FirstOrDefault(a => a.Nome.ToLower() == name && a.Sobrenome.ToLower() == lastname);
      }
      else if (string.IsNullOrEmpty(name) == false)
      {
        aluno = _context.Alunos.FirstOrDefault(a => a.Nome.ToLower() == name);
      }
      else if (string.IsNullOrEmpty(lastname) == false)
      {
        aluno = _context.Alunos.FirstOrDefault(a => a.Sobrenome.ToLower() == lastname);
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
      _context.Add(aluno);
      _context.SaveChanges();
      return Ok(aluno);
    }

    // PUT api/<AlunoController>/5
    [HttpPut()]
    public IActionResult Put(Aluno aluno)
    {
      var alunoExistente = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == aluno.Id);
      if (alunoExistente == null) return BadRequest("Aluno not found");

      _context.Update(aluno);
      _context.SaveChanges();
      return Ok(aluno);
    }

    // DELETE api/<AlunoController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var alunoExistente = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
      if (alunoExistente == null) return BadRequest("Aluno not found");

      _context.Remove(alunoExistente);
      _context.SaveChanges();
      return Ok("deu boas");
    }
  }
}
