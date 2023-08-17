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

        // I need to introduce a foreign key relationship between the Ticket and ApplicationUser tables?
        // the Ticket table has a UserId column that referennces the Id column in the ApplicationUser table.
       
        //override protected void OnModelCreating(ModelBuilder modelBuilder)
        //{ 
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Ticket>()
        //        .HasOne(t => t.ApplicationUser) // Each Ticket has one ApplicationUserEmail
        //        .WithMany(g => g.Tickets)
        //        .HasForeignKey(s => s.ApplicationUserEmail).HasPrincipalKey(u=>u.Email);
        //}

    }
}

