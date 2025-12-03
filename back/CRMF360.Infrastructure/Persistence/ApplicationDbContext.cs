using CRMF360.Application.Abstractions;
using CRMF360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMF360.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Empresa> Empresas => Set<Empresa>();
        public DbSet<PersonaEmpresa> PersonasEmpresa => Set<PersonaEmpresa>();
        public DbSet<Proyecto>Proyectos => Set<Proyecto>();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.FullName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            // UserRole
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            });
        }
    }
}
