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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.V1.Controllers
{
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class ProfessorController : ControllerBase
  {
    private readonly IRepository _repo;
    private readonly IMapper _mapper;
    public ProfessorController(IRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    /// <summary>
    /// Método responsável por retornar todos os professores
    /// </summary>
    // GET: api/<ProfessorController>
    [HttpGet]
    public IActionResult Get()
    {
      return Ok(_repo.GetAllProfessores(true));
    }

    /// <summary>
    /// Método responsável por retornar um professor a partir do ID
    /// </summary>
    // GET api/<ProfessorController>/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var professor = _repo.GetProfessorById(id);
      return Ok(_mapper.Map<ProfessorDto>(professor));
    }

    /// <summary>
    /// Método responsável por receber os dados do professor e gravar no banco de dados
    /// </summary>
    // POST api/<ProfessorController>
    [HttpPost]
    public IActionResult Post(ProfessorRegistrarDto professorRegistrarDto)
    {
      var professor = _mapper.Map<Professor>(professorRegistrarDto);
      _repo.Add(professor);
      _repo.SaveChanges();
      return Ok(_mapper.Map<ProfessorDto>(professor));
    }

    /// <summary>
    /// Método responsável por atualizar os dados do professor 
    /// </summary>
    // PUT api/<ProfessorController>/5
    [HttpPut("id")]
    public IActionResult Put(int id, ProfessorRegistrarDto professorRegistrarDto)
    {
      var professorExistente = _repo.GetProfessorById(id);
      if (professorExistente == null) return BadRequest("Professor não encontrado");

      var professor = _mapper.Map<Professor>(professorRegistrarDto);
      professor.Id = id;
      _repo.Update(professor);
      _repo.SaveChanges();
      return Ok(_mapper.Map<ProfessorDto>(professor));
    }

    /// <summary>
    /// Método responsável por deletar o professor a partir de um ID
    /// </summary>
    // DELETE api/<ProfessorController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var professor = _repo.GetProfessorById(id);
      if (professor == null) return BadRequest("Professor não encontrado");

      _repo.Remove(professor);
      _repo.SaveChanges();
      return Ok("Professor removido");
    }
  }
}
