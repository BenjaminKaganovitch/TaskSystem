using System.ComponentModel.DataAnnotations;

namespace SupportTicketSystem.Models
{
    // BaseAuthViewModel class is a base class for LoginViewModel and RegisterViewModel classes.
    public class BaseAuthViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public class LoginViewModel : BaseAuthViewModel
        {
            
        }

        // RegisterViewModel class is derived from BaseAuthViewModel class.
        public class RegisterViewModel : BaseAuthViewModel
        {
            [Required]
            [Display(Name = "User Role")]
            public string UserRole { get; set; } 
        }



    }
}
