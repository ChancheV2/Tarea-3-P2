using AutoMapper;
using ControlDeEvaluacionesEscolares.Application.DTOs;
using ControlDeEvaluacionesEscolares.Domain.Entities;

namespace ControlDeEvaluacionesEscolares.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Estudiante, EstudianteDto>();
            CreateMap<EstudianteDto, Estudiante>();

            CreateMap<Curso, CursoDto>();
            CreateMap<CursoDto, Curso>();

            CreateMap<Evaluacion, EvaluacionDto>()
                .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src =>
                    src.Estudiante != null ? $"{src.Estudiante.Nombre} {src.Estudiante.Apellido}" : string.Empty))
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src =>
                    src.Curso != null ? src.Curso.Nombre : string.Empty));
            CreateMap<EvaluacionDto, Evaluacion>();
        }
    }
}