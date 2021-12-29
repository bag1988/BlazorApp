using ServiceGrpc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Grpc.Data
{
    public class MessagesDbContext:DbContext
    {
        public MessagesDbContext(DbContextOptions<MessagesDbContext> options):base(options)
        { 
                Database.Migrate();
        }

        public DbSet<MessageModel> Messages {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MessageModel>().HasKey(b => b.Id);
            builder.Entity<MessageModel>().Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
