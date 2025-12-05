using CRMF360.Domain.Entities;
using CRMF360.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMF360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class EmpresasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmpresasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Empresas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas([FromQuery] string? search)
    {
        var query = _context.Empresas
            .Include(e => e.Personas)
            .Include(e => e.Proyectos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.RazonSocial.Contains(search) ||
                (e.NombreFantasia != null && e.NombreFantasia.Contains(search)) ||
                (e.Cuit != null && e.Cuit.Contains(search)));
        }

        var result = await query
            .OrderBy(e => e.RazonSocial)
            .ToListAsync();

        return Ok(result);
    }

    // GET: api/Empresas/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Empresa>> GetEmpresa(int id)
    {
        var empresa = await _context.Empresas
            .Include(e => e.Personas)
            .Include(e => e.Proyectos)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (empresa == null)
            return NotFound();

        return Ok(empresa);
    }

    // POST: api/Empresas
    [HttpPost]
    [Authorize(Roles = "Admin")] // opcional
    public async Task<ActionResult<Empresa>> CrearEmpresa([FromBody] Empresa empresa)
    {
        empresa.Id = 0;
        empresa.FechaAlta = DateTime.UtcNow;
        empresa.Activa = true;

        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresa);
    }

    // PUT: api/Empresas/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarEmpresa(int id, [FromBody] Empresa empresa)
    {
        if (id != empresa.Id)
            return BadRequest("El id de la URL no coincide con el del cuerpo.");

        var existing = await _context.Empresas.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.RazonSocial = empresa.RazonSocial;
        existing.NombreFantasia = empresa.NombreFantasia;
        existing.Cuit = empresa.Cuit;
        existing.Email = empresa.Email;
        existing.Telefono = empresa.Telefono;
        existing.Direccion = empresa.Direccion;
        existing.Activa = empresa.Activa;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE lógico: api/Empresas/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BorrarEmpresa(int id)
    {
        var empresa = await _context.Empresas.FindAsync(id);
        if (empresa == null)
            return NotFound();

        empresa.Activa = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
