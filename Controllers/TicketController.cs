using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;

namespace SupportTicketSystem.Controllers
{
    public class TicketController : Controller
    {
        private readonly SupportTicketSystemContext _context;

        public TicketController(SupportTicketSystemContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            // Find all tickets that belong to the currently logged in user
            var userTickets = await _context.Ticket.Where(t => t.UserId == loggedInUserId).ToListAsync();

            if (User.IsInRole("Admin"))
            {
                // If the user is an admin, find all tickets
                userTickets = await _context.Ticket.ToListAsync();
            }

            return View(userTickets);
        }


        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the ticket with the specified ID
            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.ID == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }


        [Authorize(Roles = "Admin, User")]
        public IActionResult Create()
        {
            // Get the ID of the currently logged in user
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            // Pass the ID of the currently logged in user to the view
            return View(new CreateTicketViewModel() {UserId = loggedInUserId });
        }


        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,CreationDate,Priority,Status,UserId")] Ticket ticket)
        {
            // Set the creation date to the current date
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }


        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }


        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // The Bind attribute is used to prevent overposting.
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,CreationDate,Priority,Status")] Ticket ticket) 
        {
            // The id parameter is used to check if the ticket exists in the database
            if (id != ticket.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }

                // The DbUpdateConcurrencyException is thrown if the ticket does not exist in the database
                catch (DbUpdateConcurrencyException) 
                {
                    // Check if the ticket exists in the database
                    if (!TicketExists(ticket.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }


        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the ticket with the specified ID. The Include method is used to include the user that created the ticket
            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            // Find the ticket with the specified ID
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.ID == id);
        }
        [Authorize(Roles = "Admin, User")]
        public IActionResult Filter()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public IActionResult Filter(FilterViewModel model)
        {
            
            IEnumerable<Ticket> filteredTickets = _context.Ticket.ToArray(); 

            // Define an array of filter properties, each containing the property name and its corresponding selected value from the model.
            var filterProperties = new[]
            {
                new { PropertyName = "Priority", PropertyValue = model.SelectedPriority },
                new { PropertyName = "Status", PropertyValue = model.SelectedStatus }
            };

            if (User.IsInRole("User"))
            {
                // If the user is not an admin, only show tickets that belong to the currently logged in user
                filteredTickets = filteredTickets.Where(t => t.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            
            foreach (var filter in filterProperties)
            {
                
                if (!string.IsNullOrEmpty(filter.PropertyValue))
                {
                    // Filter the tickets based on the selected filter properties
                    filteredTickets = filteredTickets.Where(t => t.GetType().GetProperty(filter.PropertyName)!.GetValue(t, null)!.ToString() == filter.PropertyValue);
                }
            }

            var filteredTicketList = filteredTickets.ToList();
            return View("Index", filteredTicketList);
        }

    }

}