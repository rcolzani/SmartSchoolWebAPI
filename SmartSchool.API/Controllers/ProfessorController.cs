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
  public class ProfessorController : ControllerBase
  {
    private readonly IRepository _repo;

    public ProfessorController(IRepository repo)
    {
      _repo = repo;
    }
    // GET: api/<ProfessorController>
    [HttpGet]
    public IActionResult Get()
    {
      return Ok(_repo.GetAllProfessores(true));
    }

    // GET api/<ProfessorController>/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var professor = _repo.GetProfessorById(id);
      return Ok(professor);
    }

    // POST api/<ProfessorController>
    [HttpPost]
    public IActionResult Post(Professor professor)
    {
      _repo.Add(professor);
      _repo.SaveChanges();
      return Ok(professor);
    }

    // PUT api/<ProfessorController>/5
    [HttpPut()]
    public IActionResult Put(Professor professor)
    {
      var professorExistente = _repo.GetProfessorById(professor.Id);
      if (professorExistente == null) return BadRequest("Professor não encontrado");

      _repo.Update(professor);
      _repo.SaveChanges();
      return Ok(professor);
    }

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
