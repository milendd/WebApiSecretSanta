using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SantaSystem.Web.Models.Groups
{
    public class SendInvitationResultModel
    {
        public int InvitationId { get; set; }

        public int GroupId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}