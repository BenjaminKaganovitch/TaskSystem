using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SupportTicketSystem.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using static SupportTicketSystem.Models.BaseAuthViewModel;

namespace SupportTicketSystem.Controllers
{
    public class AccountController : Controller
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
       
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly SupportTicketSystemContext _context; 

        // Injects the UserManager and SignInManager services into the controller
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, SupportTicketSystemContext context)
        {
            _userManager = userManager;
           
            _signInManager = signInManager;
            
            _context = context; 
        }


        
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        // Allow unauthenticated users to access the Login action
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, };
                var result = await _userManager.CreateAsync(user, model.Password);
                // Check if the user was successfully created
                if (result.Succeeded)
                {
                    // Assign role based on the selected UserRole
                    if (model.UserRole == "Admin")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
               



                    user.Email = model.Email;
                    user.EmailConfirmed = true;

                    await _userManager.UpdateAsync(user);

                    // Sign in the user using the SignInManager class
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect to home after successful registration
                    return RedirectToAction("Index", "Home"); 
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [AllowAnonymous] 
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

  
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            // Set the returnUrl to the home page if it is null
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // The lockoutOnFailure parameter specifies whether a user account should be locked if the sign-in fails.
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false); 
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Account"); 
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Get the currently authenticated user
            var user = await _userManager.GetUserAsync(User);
            // Get the tickets created by the user
            var tickets = _context.Ticket.Where(t => t.UserId == user.Id)
                                         .Select(t => new UserTicketViewModel
                                         {
                                             // Map the Ticket object to a UserTicketViewModel object
                                             // Execute the query and return the results as a list
                                             ID = t.ID,
                                             Title = t.Title,
                                             Description = t.Description,
                                             CreationDate = t.CreationDate,
                                             Priority = t.Priority,
                                             Status = t.Status
                                         })
                                         .ToList(); 

            // Create a ProfileViewModel object and pass it to the view
            var profileViewModel = new ProfileViewModel
            {
                Email = user.Email,
                Tickets = tickets
            };
           
            return View(profileViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
