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
    private readonly SmartContext _context;

    public ProfessorController(SmartContext context)
    {
      _context = context;
    }
    // GET: api/<ProfessorController>
    [HttpGet]
    public IActionResult Get()
    {
      return Ok(_context.Professores);
    }

    // GET api/<ProfessorController>/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var professor = _context.Professores.FirstOrDefault(a => a.Id == id);
      return Ok(professor);
    }

    // POST api/<ProfessorController>
    [HttpPost]
    public IActionResult Post(Professor professor)
    {
      _context.Add(professor);
      _context.SaveChanges();
      return Ok(professor);
    }

    // PUT api/<ProfessorController>/5
    [HttpPut()]
    public IActionResult Put(Professor professor)
    {
      var professorExistente = _context.Professores.AsNoTracking().FirstOrDefault(p => p.Id == professor.Id);
      if (professorExistente == null) return BadRequest("Professor não encontrado");

      _context.Professores.Update(professor);
      _context.SaveChanges();
      return Ok(professor);
    }

    // DELETE api/<ProfessorController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var professor = _context.Professores.FirstOrDefault(p => p.Id == id);
      if (professor == null) return BadRequest("Professor não encontrado");

      _context.Professores.Remove(professor);
      _context.SaveChanges();
      return Ok("Professor removido");
    }
  }
}
