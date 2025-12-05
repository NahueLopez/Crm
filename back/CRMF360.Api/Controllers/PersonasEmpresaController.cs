using CRMF360.Application.PersonasEmpresa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMF360.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonasEmpresaController : ControllerBase
{
    private readonly IPersonaEmpresaService _service;

    public PersonasEmpresaController(IPersonaEmpresaService service)
    {
        _service = service;
    }

    [HttpGet("by-empresa/{empresaId:int}")]
    public async Task<ActionResult<IEnumerable<PersonaEmpresaDto>>> GetByEmpresa(int empresaId)
    {
        var personas = await _service.GetByEmpresaAsync(empresaId);
        return Ok(personas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonaEmpresaDto>> GetPersona(int id)
    {
        var persona = await _service.GetByIdAsync(id);
        if (persona == null) return NotFound();
        return Ok(persona);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PersonaEmpresaDto>> CrearPersona([FromBody] CreatePersonaEmpresaRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetPersona), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarPersona(int id, [FromBody] UpdatePersonaEmpresaRequest request)
    {
        var ok = await _service.UpdateAsync(id, request);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BorrarPersona(int id)
    {
        var ok = await _service.SoftDeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
