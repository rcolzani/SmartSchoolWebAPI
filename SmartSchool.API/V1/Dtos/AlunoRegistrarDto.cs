using System;

namespace SmartSchool.API.V1.Dtos
{
  public class AlunoRegistrarDto
  {
    /// <summary>
    /// Utilizar ID apenas para atualização.<!-- -->Na criação de um novo aluno o Id será criado automaticamente.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Matrícula obtida junto a escola
    /// </summary>
    public int Matricula { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Telefone { get; set; }
    public DateTime DataNasc { get; set; }
    public DateTime DataIni { get; set; } = DateTime.Now;
    public DateTime? DataFim { get; set; } = null;
    public bool Ativo { get; set; } = true;
  }
}