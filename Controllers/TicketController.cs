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
    [Authorize(Roles = "Admin, User")]
    public class TicketController : Controller
    {
        private readonly SupportTicketSystemContext _context;

        public TicketController(SupportTicketSystemContext context)
        {
            _context = context;
        }

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

        public IActionResult Create()
        {
            // Get the ID of the currently logged in user
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            ViewBag.UserId = loggedInUserId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,CreationDate,Priority,Status,UserId")] Ticket ticket)
        {
            // NOTE: we do need all the fields to be entered otherwise we wont process, and we will stay on the Create Screen.
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));
            }
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new CreateTicketViewModel() { UserId = loggedInUserId });
        }

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

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && ticket.UserId != loggedInUserId)

            {
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ID,Title,Description,CreationDate,Priority,Status")] Ticket ticket) 
        {
            // The id parameter is used to check if the ticket exists in the database
            if (id != ticket.ID)
            {
                return NotFound();
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && ticket.UserId != loggedInUserId)
            {
                return RedirectToAction(nameof(Index));
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

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && ticket.UserId != loggedInUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            // Find the ticket with the specified ID
            var ticket = await _context.Ticket.FindAsync(id);
            // Fix the green swiggly line, to ensure that the ticket is not null, to prevent a cross-site request forgery attack
            if (ticket == null)
            {
                return NotFound();
            }
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && ticket.UserId != loggedInUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Ticket.Remove(ticket);



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.ID == id);

        }

        public IActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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