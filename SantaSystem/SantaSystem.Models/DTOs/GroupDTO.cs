using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaSystem.Models.DTOs
{
    public class GroupDTO
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<string> Members { get; set; }
    }
}
