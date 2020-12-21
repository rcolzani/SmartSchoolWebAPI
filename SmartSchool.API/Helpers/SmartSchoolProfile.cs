using AutoMapper;
using SmartSchool.API.Dtos;
using SmartSchool.API.Models;

namespace SmartSchool.API.Helpers
{
  public class SmartSchoolProfile : Profile
  {
    public SmartSchoolProfile()
    {
      CreateMap<Aluno, AlunoDto>()
      .ForMember(
          dest => dest.Nome,
          opt => opt.MapFrom(src => $"{src.Nome} {src.Sobrenome}")
      )
      .ForMember(dest => dest.Idade,
        opt => opt.MapFrom(src => src.DataNasc.GetCurrentAge())
        ).ReverseMap();

      CreateMap<AlunoRegistrarDto, Aluno>().ReverseMap();

      CreateMap<ProfessorDto, Professor>().ReverseMap();
      CreateMap<ProfessorRegistrarDto, Professor>().ReverseMap();
    }
  }
}