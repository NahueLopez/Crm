using CRMF360.Application.Empresas;
using CRMF360.Application.Proyectos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProyectosController : ControllerBase
{
    private readonly IProyectoService _service;

    public ProyectosController(IProyectoService service)
    {
        _service = service;
    }

    [HttpGet("by-empresa/{empresaId:int}")]
    public async Task<ActionResult<IEnumerable<ProyectoDto>>> GetByEmpresa(int empresaId)
    {
        return Ok(await _service.GetByEmpresaAsync(empresaId));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProyectoDto>> GetProyecto(int id)
    {
        var proyecto = await _service.GetByIdAsync(id);
        return proyecto is null ? NotFound() : Ok(proyecto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProyectoDto>> CrearProyecto([FromBody] CreateProyectoRequest request)
    {
        var proyecto = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetProyecto), new { id = proyecto.Id }, proyecto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarProyecto(int id, [FromBody] UpdateProyectoRequest request)
    {
        return await _service.UpdateAsync(id, request)
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BorrarProyecto(int id)
    {
        return await _service.DeleteAsync(id)
            ? NoContent()
            : NotFound();
    }
}
