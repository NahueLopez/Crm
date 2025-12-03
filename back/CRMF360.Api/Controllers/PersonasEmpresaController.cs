using CRMF360.Domain.Entities;
using CRMF360.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMF360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonasEmpresaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PersonasEmpresaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/PersonasEmpresa/by-empresa/3
    [HttpGet("by-empresa/{empresaId:int}")]
    public async Task<ActionResult<IEnumerable<PersonaEmpresa>>> GetByEmpresa(int empresaId)
    {
        var personas = await _context.PersonasEmpresa
            .Where(p => p.EmpresaId == empresaId && p.Activa)
            .ToListAsync();

        return Ok(personas);
    }

    // GET: api/PersonasEmpresa/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonaEmpresa>> GetPersona(int id)
    {
        var persona = await _context.PersonasEmpresa.FindAsync(id);
        if (persona == null)
            return NotFound();

        return Ok(persona);
    }

    // POST: api/PersonasEmpresa
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PersonaEmpresa>> CrearPersona([FromBody] PersonaEmpresa persona)
    {
        // Validamos que la empresa exista
        var empresa = await _context.Empresas.FindAsync(persona.EmpresaId);
        if (empresa == null)
            return BadRequest($"No existe la EmpresaId={persona.EmpresaId}");

        persona.Id = 0;
        persona.Activa = true;

        _context.PersonasEmpresa.Add(persona);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
    }

    // PUT: api/PersonasEmpresa/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarPersona(int id, [FromBody] PersonaEmpresa persona)
    {
        if (id != persona.Id)
            return BadRequest("Id de URL y cuerpo no coinciden.");

        var existing = await _context.PersonasEmpresa.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.NombreCompleto = persona.NombreCompleto;
        existing.RolEnEmpresa = persona.RolEnEmpresa;
        existing.Email = persona.Email;
        existing.Telefono = persona.Telefono;
        existing.Principal = persona.Principal;
        existing.Activa = persona.Activa;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE lógico: api/PersonasEmpresa/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BorrarPersona(int id)
    {
        var persona = await _context.PersonasEmpresa.FindAsync(id);
        if (persona == null)
            return NotFound();

        persona.Activa = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
