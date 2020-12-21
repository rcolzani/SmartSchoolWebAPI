using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Dtos;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AlunoController : ControllerBase
  {
    private readonly IRepository _repo;
    private readonly IMapper _mapper;
    public AlunoController(IRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    // GET: api/<AlunoController>
    [HttpGet]
    public IActionResult Get()
    {
      var alunos = _repo.GetAllAlunos(true);
      return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
    }

    //Como existem duas rotas get iguais é necessário especificar o parâmetro de uma delas. Por padrão todas são string, então é necessário especificar a int
    // GET api/<AlunoController>/5
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
      var aluno = _repo.GetAlunoById(id);
      if (aluno == null) return BadRequest("Aluno não encontrado");
      return Ok(_mapper.Map<AlunoDto>(aluno));
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
      return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
    }

    // POST api/<AlunoController>
    [HttpPost]
    public IActionResult Post(AlunoRegistrarDto alunoRegistrar)
    {
      var aluno = _mapper.Map<Aluno>(alunoRegistrar);
      _repo.Add(aluno);
      if (_repo.SaveChanges())
      {
        return Created($"/api/aluno/{alunoRegistrar.Id}", _mapper.Map<AlunoDto>(aluno));
      }
      return BadRequest("Aluno não cadastrado");
    }

    // PUT api/<AlunoController>/5
    [HttpPut()]
    public IActionResult Put(AlunoRegistrarDto aluno)
    {
      var alunoExistente = _repo.GetAlunoById(aluno.Id);
      if (alunoExistente == null) return BadRequest("Aluno not found");

      _repo.Update(aluno);
      _repo.SaveChanges();
      return Ok(_mapper.Map<AlunoDto>(aluno));
    }

    // DELETE api/<AlunoController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var alunoExistente = _repo.GetAlunoById(id);
      if (alunoExistente == null) return BadRequest("Aluno not found");

      _repo.Remove(alunoExistente);
      _repo.SaveChanges();
      return Ok(_mapper.Map<IEnumerable<AlunoDto>>(_repo.GetAllAlunos(true)));
    }
  }
}
