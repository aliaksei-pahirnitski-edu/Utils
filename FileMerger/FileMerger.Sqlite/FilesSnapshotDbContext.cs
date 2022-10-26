using FileMerger.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileMerger.Sqlite
{
    public class FilesSnapshotDbContext : DbContext
    {
        public FilesSnapshotDbContext(DbContextOptions<FilesSnapshotDbContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseQueryTrackingBehavior
            // optionsBuilder.UseSqlite
            // optionsBuilder.AddInterceptors()
            // optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            modelBuilder.Entity<ComparableEntity>(b =>
            {
                b.Ignore(x => x.Parent);
                b.HasKey(x => new {x.Host, x.FullName});
            });*/
                
            modelBuilder.Entity<FileEntity>(b =>
            {
                b.ToTable("Files");
                b.HasBaseType((Type)null);
                b.HasKey(x => new { x.Host, x.FullName });
                b.Ignore(x => x.Parent);
                b.Property(x => x.FullName).HasMaxLength(400).IsRequired();
                b.Property(x => x.ShortName).HasMaxLength(100).IsRequired();
                b.Property(x => x.Host).HasMaxLength(50).IsRequired();
                b.Property(x => x.Hash).HasMaxLength(100).IsRequired();
            });
        }
    }
}