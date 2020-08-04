using FreeResponse.Core.Projects;
using FreeResponse.Core.SdlcSystems;
using Microsoft.EntityFrameworkCore;

namespace FreeResponse.EntityFrameworkCore.EntityFrameworkCore
{
    public class FreeResponseDbContext: DbContext
    {
        public DbSet<SdlcSystem> SdlcSystems { get; set; }
        public DbSet<Project> Projects { get; set; }

        public FreeResponseDbContext(DbContextOptions<FreeResponseDbContext> options)
            : base(options)
        {
        }
    }
}

