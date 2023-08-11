using System;

namespace SupportTicketSystem.Models
{
    // UserTicketViewModel class is derived from BaseAuthViewModel class.
    public class UserTicketViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
