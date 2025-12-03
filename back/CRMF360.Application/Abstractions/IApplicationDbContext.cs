using CRMF360.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CRMF360.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
