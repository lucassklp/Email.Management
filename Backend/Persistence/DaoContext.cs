using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Backend.Persistence
{
    public class DaoContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerFactory loggerFactory;
        public DaoContext(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 21)));
            options.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.UseSnakeCaseNamingConvention();
            base.OnModelCreating(modelBuilder);
        }
    }
}