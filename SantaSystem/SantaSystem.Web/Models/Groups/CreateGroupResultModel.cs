using System;

namespace SantaSystem.Web.Models.Groups
{
    public class CreateGroupResultModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
