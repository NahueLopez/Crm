using CRMF360.Domain.Entities;
using CRMF360.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMF360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProyectosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProyectosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Proyectos/by-empresa/3
    [HttpGet("by-empresa/{empresaId:int}")]
    public async Task<ActionResult<IEnumerable<Proyecto>>> GetByEmpresa(int empresaId)
    {
        var proyectos = await _context.Proyectos
            .Where(p => p.EmpresaId == empresaId)
            .ToListAsync();

        return Ok(proyectos);
    }

    // GET: api/Proyectos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Proyecto>> GetProyecto(int id)
    {
        var proyecto = await _context.Proyectos
            .Include(p => p.Empresa)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proyecto == null)
            return NotFound();

        return Ok(proyecto);
    }

    // POST: api/Proyectos
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Proyecto>> CrearProyecto([FromBody] Proyecto proyecto)
    {
        // Validar empresa
        var empresa = await _context.Empresas.FindAsync(proyecto.EmpresaId);
        if (empresa == null)
            return BadRequest($"No existe la EmpresaId={proyecto.EmpresaId}");

        proyecto.Id = 0;

        _context.Proyectos.Add(proyecto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProyecto), new { id = proyecto.Id }, proyecto);
    }

    // PUT: api/Proyectos/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarProyecto(int id, [FromBody] Proyecto proyecto)
    {
        if (id != proyecto.Id)
            return BadRequest("Id de URL y cuerpo no coinciden.");

        var existing = await _context.Proyectos.FindAsync(id);
        if (existing == null)
            return NotFound();

        // (si quisieras permitir cambiar de empresa, podrías actualizar EmpresaId)
        existing.Nombre = proyecto.Nombre;
        existing.Descripcion = proyecto.Descripcion;
        existing.FechaInicio = proyecto.FechaInicio;
        existing.FechaFin = proyecto.FechaFin;
        existing.Estado = proyecto.Estado;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Proyectos/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BorrarProyecto(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto == null)
            return NotFound();

        _context.Proyectos.Remove(proyecto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
