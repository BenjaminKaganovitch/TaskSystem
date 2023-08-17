using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SupportTicketSystem.Models;
using static SupportTicketSystem.Models.BaseAuthViewModel;
using System;
using static SupportTicketSystem.Models.Ticket;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
namespace SupportTicketSystem.Data
{
    internal class SeedData
    {
        // Connect the methods: SeedTickets, SeedUsers, SeedTables to the initlize method here 
        internal static void Initialize(IServiceProvider serviceProvider)
        {
            // Check to see if the roles exist and create them if not
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedRoles(roleManager);

            using var context = new SupportTicketSystemContext(
                               serviceProvider.GetRequiredService<DbContextOptions<SupportTicketSystemContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            SeedRoles(roleManager);

            _ = SeedUsers(userManager);

            SeedTickets(context);

            
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Check to see if the Admin role exists and create it if not

            if (!roleManager.RoleExistsAsync("Admin").Result)
                _ = roleManager.CreateAsync(new IdentityRole("Admin")).Result;

            if (!roleManager.RoleExistsAsync("User").Result)
                _ = roleManager.CreateAsync(new IdentityRole("User")).Result;
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create an admin user
            var admin = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
            };

            var adminResult = await userManager.CreateAsync(admin, "Admin123!"); // Replace with your desired password
            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Create a regular user
            var user = new ApplicationUser
            {
                UserName = "user@example.com",
                Email = "user@example.com",
            };

            var userResult = await userManager.CreateAsync(user, "User123!"); // Replace with your desired password
            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }


        private static void SeedTickets(SupportTicketSystemContext context)
        {
            if (!context.Ticket.Any()) // Note its "Ticket" instead of "Tickets"
            {
                var myticket = new Ticket
                {
                    Title = "Finish ML Playlist",
                    Description = "Just watch it",
                    CreationDate = DateTime.Now,
                    Priority = "High",
                    Status = "Open",
                    UserId = "admin@example.com", // Replace with the admin user's email
                };

                var myticket1 = new Ticket
                {
                    Title = "Ask Chris why I got fired",
                    Description = "Send email",
                    CreationDate = DateTime.Now,
                    Priority = "High",
                    Status = "Open",
                    UserId = "user@example.com", // Replace with the regular user's email
                };

                var myticket2 = new Ticket
                {
                    Title = "Fail Xander",
                    Description = "delete the code from his laptop",
                    CreationDate = DateTime.Now,
                    Priority = "Low",
                    Status = "Open",
                    UserId = "admin@example.com", // Replace with the admin user's email
                };

                // Add the ticket objects to the context
                context.Ticket.Add(myticket);
                context.Ticket.Add(myticket1);
                context.Ticket.Add(myticket2);

                context.SaveChanges(); // Save changes to the database
            }
        }

    }
}
