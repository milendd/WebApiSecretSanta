using System.ComponentModel.DataAnnotations;

namespace SantaSystem.Web.Models.Groups
{
    public class AcceptInvitationViewModel
    {
        [Required]
        public string GroupName { get; set; }
    }
}