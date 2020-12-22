using AutoMapper;
using SmartSchool.API.V2.Dtos;
using SmartSchool.API.Models;
using SmartSchool.API.Helpers;

namespace SmartSchool.API.V2.Profiles
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
        );

      CreateMap<Aluno, AlunoDto>().ReverseMap();
      CreateMap<AlunoRegistrarDto, Aluno>().ReverseMap();

      CreateMap<ProfessorDto, Professor>().ReverseMap();
      CreateMap<ProfessorRegistrarDto, Professor>().ReverseMap();
    }
  }
}