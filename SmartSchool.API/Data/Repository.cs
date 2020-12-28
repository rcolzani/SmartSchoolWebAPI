using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Helpers;
using SmartSchool.API.Models;

namespace SmartSchool.API.Data
{
    public class Repository : IRepository
    {
        private readonly SmartContext _context;
        public Repository(SmartContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }

        public async Task<User> GetUser(string userName, string userPassword)
        {
            IQueryable<User> query = _context.Users;

            query = query.Where(u => u.Username.ToLower() == userName.ToLower() && u.Password == userPassword);
            return await query.FirstOrDefaultAsync();
        }

        public Aluno[] GetAllAlunos(bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;
            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                             .ThenInclude(ad => ad.Disciplina)
                             .ThenInclude(p => p.Professor);
            }
            query = query.AsNoTracking()
                         .OrderBy(a => a.Id);
            return query.ToArray();
        }

        public async Task<PageList<Aluno>> GetAllAlunosAsync(
          PageParams pageParams,
          bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;
            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                             .ThenInclude(ad => ad.Disciplina)
                             .ThenInclude(p => p.Professor);
            }
            query = query.AsNoTracking()
                         .OrderBy(a => a.Id);

            if (!string.IsNullOrEmpty(pageParams.Nome))
            {
                query = query.Where(aluno => aluno.Nome.ToUpper().Contains(pageParams.Nome.ToUpper())
                || aluno.Sobrenome.ToUpper().Contains(pageParams.Nome.ToUpper()));
            }

            if (pageParams.Matricula > 0)
            {
                query = query.Where(aluno => aluno.Matricula == pageParams.Matricula);
            }
            if (pageParams != null)
            {
                query = query.Where(aluno => aluno.Ativo == (pageParams.Ativo != 0));
            }

            return await PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public Aluno[] GetAllAlunosByDisciplina(int discplinaId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;
            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                             .ThenInclude(ad => ad.Disciplina)
                             .ThenInclude(p => p.Professor);
            }
            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(aluno => aluno.AlunosDisciplinas.Any(ad => ad.DisciplinaId == discplinaId));
            return query.ToArray();
        }

        public Aluno GetAlunoById(int alunoId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;
            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                             .ThenInclude(ad => ad.Disciplina)
                             .ThenInclude(p => p.Professor);
            }
            query = query.AsNoTracking()
                          .Where(al => al.Id == alunoId)
                         .OrderBy(a => a.Id);
            return query.FirstOrDefault();
        }
        public Aluno[] GetAlunoByName(string name, string lastname)
        {
            if (string.IsNullOrEmpty(name) == false) name = name.ToLower();
            if (string.IsNullOrEmpty(lastname) == false) lastname = lastname.ToLower();
            IQueryable<Aluno> query = _context.Alunos;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(lastname))
            {
                return new Aluno[0];
            }
            else if (string.IsNullOrEmpty(name) == false && string.IsNullOrEmpty(lastname) == false)
            {
                query = query.Where(a => a.Nome.ToLower().Contains(name) && a.Sobrenome.ToLower().Contains(lastname));
            }
            else if (string.IsNullOrEmpty(name) == false)
            {
                query = query.Where(a => a.Nome.ToLower().Contains(name));
            }
            else if (string.IsNullOrEmpty(lastname) == false)
            {
                query = query.Where(a => a.Sobrenome.ToLower().Contains(lastname));
            }

            query = query.Include(a => a.AlunosDisciplinas)
                         .ThenInclude(ad => ad.Disciplina)
                         .ThenInclude(p => p.Professor);

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id);
            return query.ToArray();
        }
        public Professor[] GetAllProfessores(bool includeAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;
            if (includeAlunos)
            {
                query = query.Include(a => a.Disciplinas)
                             .ThenInclude(ad => ad.AlunosDisciplinas)
                             .ThenInclude(aluno => aluno.Aluno);
            }
            query = query.AsNoTracking()
                         .OrderBy(a => a.Id);
            return query.ToArray();
        }

        public Professor[] GetAllProfessoresByDisciplina(int discplinaId, bool includeAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;
            if (includeAlunos)
            {
                query = query.Include(a => a.Disciplinas)
                             .ThenInclude(ad => ad.AlunosDisciplinas)
                             .ThenInclude(aluno => aluno.Aluno);
            }
            query = query.AsNoTracking()
                         .Where(prof => prof.Disciplinas.Any(ad => ad.Id == discplinaId))
                         .OrderBy(a => a.Id);
            return query.ToArray();
        }

        public Professor GetProfessorById(int professorId)
        {
            IQueryable<Professor> query = _context.Professores;
            query = query.Include(a => a.Disciplinas)
                         .ThenInclude(ad => ad.AlunosDisciplinas)
                         .ThenInclude(aluno => aluno.Aluno);
            query = query.AsNoTracking()
                         .Where(prof => prof.Id == professorId)
                         .OrderBy(a => a.Id);
            return query.FirstOrDefault();
        }
    }
}