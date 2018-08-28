using System;

namespace SantaSystem.Models.DTOs
{
    public class InvitationDTO
    {
        public int InvitationId { get; set; }
        
        public int GroupId { get; set; }
        
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
