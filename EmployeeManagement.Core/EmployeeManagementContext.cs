using EmployeeManagement.Core.Constants;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Core
{
    public class EmployeeManagementContext : DbContext
    {
        public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }

        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class => Set<TEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .ToTable("Employees")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(11);

            modelBuilder.Entity<Role>()
                .ToTable("Roles")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<Role>()
                .Property(_ => _.Id)
                .IsRequired();

            modelBuilder.Entity<EmployeeRole>()
                .ToTable("EmployeeRoles")
                .HasKey(er => new { er.EmployeeId, er.RoleId });

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.Roles)
                .HasForeignKey(er => er.EmployeeId);

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(er => er.RoleId);


            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = RoleType.Director },
                new Role { Id = 2, Name = RoleType.IT },
                new Role { Id = 3, Name = RoleType.Support },
                new Role { Id = 4, Name = RoleType.Accounting },
                new Role { Id = 5, Name = RoleType.Analyst },
                new Role { Id = 6, Name = RoleType.Sales }
            );
        }
    }
}
