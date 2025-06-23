using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShoppingSessionDto
    {
        public Guid SessionId { get; set; }
        public string? SessionName { get; set; }
        public string Status { get; set; } = "Active";
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }

        // Parameterless constructor for AutoMapper
        public ShoppingSessionDto()
        {
        }

        public ShoppingSessionDto(Guid sessionId, string? sessionName, string status, int totalItems, DateTime createdAt, DateTime? completedAt = null, string? notes = null)
        {
            SessionId = sessionId;
            SessionName = sessionName;
            Status = status;
            TotalItems = totalItems;
            CreatedAt = createdAt;
            CompletedAt = completedAt;
            Notes = notes;
        }
    }
}
