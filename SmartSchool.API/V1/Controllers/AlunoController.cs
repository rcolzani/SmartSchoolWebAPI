using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.V1.Dtos;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.V1.Controllers
{
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class AlunoController : ControllerBase
  {
    private readonly IRepository _repo;
    private readonly IMapper _mapper;
    /// <summary>
    /// Controller de alunos
    /// </summary>
    public AlunoController(IRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }


    /// <summary>
    /// Método responsável por retornar todos os alunos
    /// </summary>
    // GET: api/<AlunoController>
    [HttpGet]
    public IActionResult Get()
    {
      var alunos = _repo.GetAllAlunos(true);
      return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
    }

    /// <summary>
    /// Método responsável por retornar um aluno a partir do ID
    /// </summary>
    //Como existem duas rotas get iguais é necessário especificar o parâmetro de uma delas. Por padrão todas são string, então é necessário especificar a int
    // GET api/<AlunoController>/5
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
      var aluno = _repo.GetAlunoById(id);
      if (aluno == null) return BadRequest("Aluno não encontrado");
      return Ok(_mapper.Map<AlunoDto>(aluno));
    }

    /// <summary>
    /// Método responsável por retornar uma lista de alunos filtrando por partes do nome ou sobrenome
    /// </summary>
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

    /// <summary>
    /// Método responsável por receber o POST do aluno e inserir no banco de dados
    /// </summary>
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

    /// <summary>
    /// Método responsável por atualizar os dados do aluno
    /// </summary>
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

    /// <summary>
    /// Método responsável por deletar um aluno a partir do ID
    /// </summary>
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
