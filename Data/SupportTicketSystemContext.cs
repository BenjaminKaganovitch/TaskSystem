using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Models;

namespace SupportTicketSystem.Data
{
    // This class also acts as part 2 of the seed data class
    public class SupportTicketSystemContext : IdentityDbContext<ApplicationUser>
    {
        // The SupportTicketSystemContext class represents the database context for your application. It manages interactions with the database, including saving and retrieving data.
        public SupportTicketSystemContext(DbContextOptions<SupportTicketSystemContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Ticket { get; set; } 

        // identity related DbSet properties
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
        public DbSet<IdentityUserToken<string>> UserTokens { get; set; }
        public DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This method is called when the model for a derived context has been initialized, but before the model has been locked down and used to initialize the context.
            base.OnModelCreating(modelBuilder);

            // The code below is used to create a composite key for the Ticket table. The composite key is made up of the TicketId and UserId properties.
            modelBuilder.Entity<Ticket>()
                .HasOne<ApplicationUser>(t => t.ApplicationUser)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

