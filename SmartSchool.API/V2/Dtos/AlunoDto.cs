namespace SmartSchool.API.V2.Dtos
{
  public class AlunoDto
  {
    public int Id { get; set; }
    public int Matricula { get; set; }
    /// <summary>
    /// Será retornada a idade se baseando na data de nascimento e na data atual
    /// </summary>
    public int Idade { get; set; }
    public string Nome { get; set; }

  }
}