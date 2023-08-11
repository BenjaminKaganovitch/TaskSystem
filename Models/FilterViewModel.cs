using System.ComponentModel.DataAnnotations;

namespace SupportTicketSystem.Models
{
    public class FilterViewModel
    {
        [Display(Name = "Priority")]
        public string SelectedPriority { get; set; }
        [Display(Name = "Status")]
        public string SelectedStatus { get; set; }
    }
}
