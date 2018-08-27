using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SantaSystem.Web.Models.Groups
{
    public class SendInvitationViewModel
    {
        [Required]
        public string ToUsername { get; set; }

        [Required]
        public string GroupName { get; set; }
    }
}