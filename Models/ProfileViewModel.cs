using System.ComponentModel.DataAnnotations;

namespace SupportTicketSystem.Models
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<UserTicketViewModel> Tickets { get; set; }
    }
}
