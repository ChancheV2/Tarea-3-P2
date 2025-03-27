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
    public class EstudiantesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EstudiantesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<StatusDto>> GetAll()
        {
            try
            {
                var estudiantes = await _context.Estudiantes.ToListAsync();
                var estudiantesDto = _mapper.Map<List<EstudianteDto>>(estudiantes);
                return Ok(StatusDto.Ok("Estudiantes obtenidos con éxito", estudiantesDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al obtener estudiantes: {ex.Message}"));
            }
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusDto>> GetById(int id)
        {
            try
            {
                var estudiante = await _context.Estudiantes.FindAsync(id);

                if (estudiante == null)
                {
                    return NotFound(StatusDto.Error($"Estudiante con ID {id} no encontrado"));
                }

                var estudianteDto = _mapper.Map<EstudianteDto>(estudiante);
                return Ok(StatusDto.Ok("Estudiante obtenido con éxito", estudianteDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al obtener el estudiante: {ex.Message}"));
            }
        }

        // POST: api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<StatusDto>> Create(EstudianteDto estudianteDto)
        {
            try
            {
                var estudiante = _mapper.Map<Estudiante>(estudianteDto);
                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync();

                estudianteDto.Id = estudiante.Id;
                return CreatedAtAction(nameof(GetById), new { id = estudiante.Id },
                    StatusDto.Ok("Estudiante creado con éxito", estudianteDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al crear el estudiante: {ex.Message}"));
            }
        }

        // PUT: api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<StatusDto>> Update(int id, EstudianteDto estudianteDto)
        {
            if (id != estudianteDto.Id)
            {
                return BadRequest(StatusDto.Error("El ID no coincide con el estudiante a actualizar"));
            }

            try
            {
                var estudiante = await _context.Estudiantes.FindAsync(id);
                if (estudiante == null)
                {
                    return NotFound(StatusDto.Error($"Estudiante con ID {id} no encontrado"));
                }

                _mapper.Map(estudianteDto, estudiante);
                _context.Entry(estudiante).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(StatusDto.Ok("Estudiante actualizado con éxito", estudianteDto));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudianteExists(id))
                {
                    return NotFound(StatusDto.Error($"Estudiante con ID {id} no encontrado"));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al actualizar el estudiante: {ex.Message}"));
            }
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StatusDto>> Delete(int id)
        {
            try
            {
                var estudiante = await _context.Estudiantes.FindAsync(id);
                if (estudiante == null)
                {
                    return NotFound(StatusDto.Error($"Estudiante con ID {id} no encontrado"));
                }

                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();

                return Ok(StatusDto.Ok($"Estudiante con ID {id} eliminado con éxito"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al eliminar el estudiante: {ex.Message}"));
            }
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}