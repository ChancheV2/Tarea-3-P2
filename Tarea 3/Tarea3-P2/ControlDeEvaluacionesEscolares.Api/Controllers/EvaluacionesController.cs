using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ControlDeEvaluacionesEscolares.Application.DTOs;
using ControlDeEvaluacionesEscolares.Domain.Common;
using ControlDeEvaluacionesEscolares.Domain.Data;
using ControlDeEvaluacionesEscolares.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlDeEvaluacionesEscolares.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EvaluacionesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Evaluaciones
        [HttpGet]
        public async Task<ActionResult<StatusDto>> GetAll()
        {
            try
            {
                var evaluaciones = await _context.Evaluaciones
                    .Include(e => e.Estudiante)
                    .Include(e => e.Curso)
                    .ToListAsync();

                var evaluacionesDto = _mapper.Map<List<EvaluacionDto>>(evaluaciones);
                return Ok(StatusDto.Ok("Evaluaciones obtenidas con éxito", evaluacionesDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al obtener evaluaciones: {ex.Message}"));
            }
        }

        // GET: api/Evaluaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusDto>> GetById(int id)
        {
            try
            {
                var evaluacion = await _context.Evaluaciones
                    .Include(e => e.Estudiante)
                    .Include(e => e.Curso)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (evaluacion == null)
                {
                    return NotFound(StatusDto.Error($"Evaluación con ID {id} no encontrada"));
                }

                var evaluacionDto = _mapper.Map<EvaluacionDto>(evaluacion);
                return Ok(StatusDto.Ok("Evaluación obtenida con éxito", evaluacionDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al obtener la evaluación: {ex.Message}"));
            }
        }

        // POST: api/Evaluaciones
        [HttpPost]
        public async Task<ActionResult<StatusDto>> Create(EvaluacionDto evaluacionDto)
        {
            try
            {
                // Verificar que existan el estudiante y el curso
                var estudiante = await _context.Estudiantes.FindAsync(evaluacionDto.EstudianteId);
                if (estudiante == null)
                {
                    return BadRequest(StatusDto.Error($"Estudiante con ID {evaluacionDto.EstudianteId} no encontrado"));
                }

                var curso = await _context.Cursos.FindAsync(evaluacionDto.CursoId);
                if (curso == null)
                {
                    return BadRequest(StatusDto.Error($"Curso con ID {evaluacionDto.CursoId} no encontrado"));
                }

                var evaluacion = _mapper.Map<Evaluacion>(evaluacionDto);
                _context.Evaluaciones.Add(evaluacion);
                await _context.SaveChangesAsync();

                evaluacionDto.Id = evaluacion.Id;
                return CreatedAtAction(nameof(GetById), new { id = evaluacion.Id },
                    StatusDto.Ok("Evaluación creada con éxito", evaluacionDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al crear la evaluación: {ex.Message}"));
            }
        }

        // PUT: api/Evaluaciones/5
        [HttpPut("{id}")]
        public async Task<ActionResult<StatusDto>> Update(int id, EvaluacionDto evaluacionDto)
        {
            if (id != evaluacionDto.Id)
            {
                return BadRequest(StatusDto.Error("El ID no coincide con la evaluación a actualizar"));
            }

            try
            {
                // Verificar que existan el estudiante y el curso
                var estudiante = await _context.Estudiantes.FindAsync(evaluacionDto.EstudianteId);
                if (estudiante == null)
                {
                    return BadRequest(StatusDto.Error($"Estudiante con ID {evaluacionDto.EstudianteId} no encontrado"));
                }

                var curso = await _context.Cursos.FindAsync(evaluacionDto.CursoId);
                if (curso == null)
                {
                    return BadRequest(StatusDto.Error($"Curso con ID {evaluacionDto.CursoId} no encontrado"));
                }

                var evaluacion = await _context.Evaluaciones.FindAsync(id);
                if (evaluacion == null)
                {
                    return NotFound(StatusDto.Error($"Evaluación con ID {id} no encontrada"));
                }

                _mapper.Map(evaluacionDto, evaluacion);
                _context.Entry(evaluacion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(StatusDto.Ok("Evaluación actualizada con éxito", evaluacionDto));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionExists(id))
                {
                    return NotFound(StatusDto.Error($"Evaluación con ID {id} no encontrada"));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al actualizar la evaluación: {ex.Message}"));
            }
        }

        // DELETE: api/Evaluaciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StatusDto>> Delete(int id)
        {
            try
            {
                var evaluacion = await _context.Evaluaciones.FindAsync(id);
                if (evaluacion == null)
                {
                    return NotFound(StatusDto.Error($"Evaluación con ID {id} no encontrada"));
                }

                _context.Evaluaciones.Remove(evaluacion);
                await _context.SaveChangesAsync();

                return Ok(StatusDto.Ok($"Evaluación con ID {id} eliminada con éxito"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al eliminar la evaluación: {ex.Message}"));
            }
        }

        private bool EvaluacionExists(int id)
        {
            return _context.Evaluaciones.Any(e => e.Id == id);
        }
    }
}