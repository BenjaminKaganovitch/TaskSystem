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

        internal static void Initialize(IServiceProvider serviceProvider)
        {
            // Check to see if the roles exist and create them if not
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedRoles(roleManager);

            using var context = new SupportTicketSystemContext(
                               serviceProvider.GetRequiredService<DbContextOptions<SupportTicketSystemContext>>());
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Check to see if the Admin role exists and create it if not

            if (!roleManager.RoleExistsAsync("Admin").Result)
                _ = roleManager.CreateAsync(new IdentityRole("Admin")).Result;

            if (!roleManager.RoleExistsAsync("User").Result)
                _ = roleManager.CreateAsync(new IdentityRole("User")).Result;
        }

        private static void SeedUsers(UserManager<RegisterViewModel> userManager)
        {
            var admin = new RegisterViewModel
            {
                UserRole = "Admin",
                Email = "ben@gmail.com",
                Password = "123qwe#E",
            };

            var user = new RegisterViewModel
            {
                UserRole = "User",
                Email = "Jerry@gmail.com",
                Password = "123qwe#D",
            };
        }

        private static void SeedTables(SupportTicketSystemContext context)
        {
            // Check to see if the tables are empty and add data if so
            if (!context.Ticket.Any())
            {
                SeedTickets(context);
            }
        }

        private static void SeedTickets(SupportTicketSystemContext context)
        {
            var myticket = new Ticket
            {
                Title = "Finish ML Playlist",
                Description = "Just watch it",
                CreationDate = DateTime.Now,
                Priority = "High",
                Status = "Open",
            };

            var myticket1 = new Ticket
            {
                Title = "Ask Chris why I got fired",
                Description = "Send email",
                CreationDate = DateTime.Now,
                Priority = "High",
                Status = "Open",
            };

            var myticket2 = new Ticket
            {
                Title = "Fail Xander",
                Description = "delete the code from his laptop",
                CreationDate = DateTime.Now,
                Priority = "Low",
                Status = "Open",
            };


            throw new NotImplementedException();
        }
    }
}
