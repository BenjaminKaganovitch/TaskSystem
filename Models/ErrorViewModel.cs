namespace SupportTicketSystem.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        // ShowRequestId method returns true if the RequestId property is not null and not empty.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}