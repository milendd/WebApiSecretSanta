using System.ComponentModel.DataAnnotations;

namespace SantaSystem.Web.Models.Links
{
    public class StartLinkViewModel
    {
        [Required]
        public string GroupName { get; set; }
    }
}