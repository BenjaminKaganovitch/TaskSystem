using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SupportTicketSystem.Models;

public class Ticket
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }

    [Required]
    public string Priority { get; set; }

    [Required]
    public string Status { get; set; }

    public string UserId { get; set; }

    [ValidateNever]
    public virtual ApplicationUser ApplicationUser { get; set; }

   
}

