using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SantaSystem.Web.Models.Groups
{
    public class AcceptInvitationResultModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Username { get; set; }
    }
}