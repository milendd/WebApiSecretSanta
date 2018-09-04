using System;

namespace SantaSystem.Models.DTOs
{
    public class InvitationDTO
    {
        public int InvitationId { get; set; }
        
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
