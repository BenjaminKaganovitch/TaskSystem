﻿using Microsoft.AspNetCore.Identity;

namespace SupportTicketSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Ticket> Tickets { get; set; } 
    }
}
