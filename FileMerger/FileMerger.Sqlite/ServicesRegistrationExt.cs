using FileMerger.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FileMerger.Sqlite
{
    public static class ServicesRegistrationExt
    {
        public static IServiceCollection AddSqliteRepo(this IServiceCollection services)
        {
            services.AddDbContext<FilesSnapshotDbContext>(opt =>
            {
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                // example: var connStr = "Data Source=E:\\ETemp\\testsqlite.sqlite";
                // opt.UseSqlite(connStr, b =>
                // {
                // }); 
                
                opt.UseSqlite();
            });
            services.AddScoped<IPersist, SqlitePersistService>();
            return services;
        }
    }
}
