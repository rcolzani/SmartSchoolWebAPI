using SmartSchool.API.Models;

namespace SmartSchool.API.Data
{
  public interface IRepository
  {
    void Add<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Remove<T>(T entity) where T : class;
    bool SaveChanges();

    Aluno[] GetAllAlunos(bool includeProfessor = false);
    Aluno[] GetAllAlunosByDisciplina(int discplinaId, bool includeProfessor = false);
    Aluno GetAlunoById(int alunoId, bool includeProfessor = false);
    Aluno[] GetAlunoByName(string name, string lastname);

    Professor[] GetAllProfessores(bool includeAlunos = false);
    Professor[] GetAllProfessoresByDisciplina(int discplinaId, bool includeAlunos = false);
    Professor GetProfessorById(int professorId);
  }
}