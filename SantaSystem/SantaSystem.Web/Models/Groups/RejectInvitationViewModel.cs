using System.ComponentModel.DataAnnotations;

namespace SantaSystem.Web.Models.Groups
{
    public class RejectInvitationViewModel
    {
        [Required]
        public string GroupName { get; set; }
    }
}