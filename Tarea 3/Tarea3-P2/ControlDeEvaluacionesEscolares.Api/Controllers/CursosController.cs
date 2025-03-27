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
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CursosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<StatusDto>> GetAll()
        {
            try
            {
                var cursos = await _context.Cursos.ToListAsync();
                var cursosDto = _mapper.Map<List<CursoDto>>(cursos);
                return Ok(StatusDto.Ok("Cursos obtenidos con éxito", cursosDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al obtener cursos: {ex.Message}"));
            }
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusDto>> GetById(int id)
        {
            try
            {
                var curso = await _context.Cursos.FindAsync(id);

                if (curso == null)
                {
                    return NotFound(StatusDto.Error($"Curso con ID {id} no encontrado"));
                }

                var cursoDto = _mapper.Map<CursoDto>(curso);
                return Ok(StatusDto.Ok("Curso obtenido con éxito", cursoDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al obtener el curso: {ex.Message}"));
            }
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<StatusDto>> Create(CursoDto cursoDto)
        {
            try
            {
                var curso = _mapper.Map<Curso>(cursoDto);
                _context.Cursos.Add(curso);
                await _context.SaveChangesAsync();

                cursoDto.Id = curso.Id;
                return CreatedAtAction(nameof(GetById), new { id = curso.Id },
                    StatusDto.Ok("Curso creado con éxito", cursoDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al crear el curso: {ex.Message}"));
            }
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<StatusDto>> Update(int id, CursoDto cursoDto)
        {
            if (id != cursoDto.Id)
            {
                return BadRequest(StatusDto.Error("El ID no coincide con el curso a actualizar"));
            }

            try
            {
                var curso = await _context.Cursos.FindAsync(id);
                if (curso == null)
                {
                    return NotFound(StatusDto.Error($"Curso con ID {id} no encontrado"));
                }

                _mapper.Map(cursoDto, curso);
                _context.Entry(curso).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(StatusDto.Ok("Curso actualizado con éxito", cursoDto));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound(StatusDto.Error($"Curso con ID {id} no encontrado"));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al actualizar el curso: {ex.Message}"));
            }
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StatusDto>> Delete(int id)
        {
            try
            {
                var curso = await _context.Cursos.FindAsync(id);
                if (curso == null)
                {
                    return NotFound(StatusDto.Error($"Curso con ID {id} no encontrado"));
                }

                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();

                return Ok(StatusDto.Ok($"Curso con ID {id} eliminado con éxito"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, StatusDto.Error($"Error al eliminar el curso: {ex.Message}"));
            }
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}