using CRMF360.Application.PersonaEmpresa;
using CRMF360.Domain.Entities;
using CRMF360.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CRMF360.Infrastructure.Services;

public class PersonaEmpresaService : IPersonaEmpresaService
{
    private readonly ApplicationDbContext _context;

    public PersonaEmpresaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PersonaEmpresaDto>> GetByEmpresaAsync(int empresaId)
    {
        var personas = await _context.PersonasEmpresa
            .Where(p => p.EmpresaId == empresaId && p.Activa)
            .ToListAsync();

        return personas.Select(p => new PersonaEmpresaDto
        {
            Id = p.Id,
            EmpresaId = p.EmpresaId,
            NombreCompleto = p.NombreCompleto,
            RolEnEmpresa = p.RolEnEmpresa,
            Email = p.Email,
            Telefono = p.Telefono,
            Principal = p.Principal,
            Activa = p.Activa
        }).ToList();
    }

    public async Task<PersonaEmpresaDto?> GetByIdAsync(int id)
    {
        var p = await _context.PersonasEmpresa.FindAsync(id);
        if (p == null) return null;

        return new PersonaEmpresaDto
        {
            Id = p.Id,
            EmpresaId = p.EmpresaId,
            NombreCompleto = p.NombreCompleto,
            RolEnEmpresa = p.RolEnEmpresa,
            Email = p.Email,
            Telefono = p.Telefono,
            Principal = p.Principal,
            Activa = p.Activa
        };
    }

    public async Task<PersonaEmpresaDto> CreateAsync(CreatePersonaEmpresaRequest request)
    {
        var empresa = await _context.Empresas.FindAsync(request.EmpresaId);
        if (empresa == null)
            throw new InvalidOperationException($"No existe EmpresaId={request.EmpresaId}");

        var entity = new PersonaEmpresa
        {
            EmpresaId = request.EmpresaId,
            NombreCompleto = request.NombreCompleto,
            RolEnEmpresa = request.RolEnEmpresa,
            Email = request.Email,
            Telefono = request.Telefono,
            Principal = request.Principal,
            Activa = true
        };

        _context.PersonasEmpresa.Add(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(entity.Id) ?? throw new InvalidOperationException("Error al crear persona.");
    }

    public async Task<bool> UpdateAsync(int id, UpdatePersonaEmpresaRequest request)
    {
        var p = await _context.PersonasEmpresa.FindAsync(id);
        if (p == null) return false;

        p.NombreCompleto = request.NombreCompleto;
        p.RolEnEmpresa = request.RolEnEmpresa;
        p.Email = request.Email;
        p.Telefono = request.Telefono;
        p.Principal = request.Principal;
        p.Activa = request.Activa;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var p = await _context.PersonasEmpresa.FindAsync(id);
        if (p == null) return false;

        p.Activa = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
