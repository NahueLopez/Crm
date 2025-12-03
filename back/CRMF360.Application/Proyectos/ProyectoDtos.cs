namespace CRMF360.Application.Empresas;

public class ProyectoDto
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }

    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    // 👇 nuevo: Id del estado + nombre legible
    public int EstadoId { get; set; }
    public string EstadoNombre { get; set; } = null!;
}

public class CreateProyectoRequest
{
    public int EmpresaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public int EstadoId { get; set; }
}

public class UpdateProyectoRequest
{
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public int EstadoId { get; set; }
}
