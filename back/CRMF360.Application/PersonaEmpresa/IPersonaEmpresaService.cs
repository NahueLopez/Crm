using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMF360.Application.PersonaEmpresa
{
    public interface IPersonaEmpresaService
    {
        Task<List<PersonaEmpresaDto>> GetByEmpresaAsync(int empresaId);
        Task<PersonaEmpresaDto?> GetByIdAsync(int id);
        Task<PersonaEmpresaDto> CreateAsync(CreatePersonaEmpresaRequest request);
        Task<bool> UpdateAsync(int id, UpdatePersonaEmpresaRequest request);
        Task<bool> SoftDeleteAsync(int id);
    }
}
