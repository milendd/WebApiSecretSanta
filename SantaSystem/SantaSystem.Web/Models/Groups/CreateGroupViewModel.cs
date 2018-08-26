using System.ComponentModel.DataAnnotations;

namespace SantaSystem.Web.Models.Groups
{
    public class CreateGroupViewModel
    {
        [Required]
        [StringLength(200)]
        public string GroupName { get; set; }
    }
}
