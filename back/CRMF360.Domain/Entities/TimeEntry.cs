namespace CRMF360.Domain.Entities;

public class TimeEntry
{
    public int Id { get; set; }

    public int ProyectoId { get; set; }
    public Proyecto Proyecto { get; set; } = null!;

    public Guid UsuarioId { get; set; }

    public DateTime Fecha { get; set; }
    public decimal Horas { get; set; }
    public string? Descripcion { get; set; }
}
