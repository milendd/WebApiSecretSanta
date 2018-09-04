using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SantaSystem.Models.DomainModels
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        
        [ForeignKey(nameof(MemberFrom))]
        public string MemberFromId { get; set; }

        public virtual User MemberFrom { get; set; }

        [ForeignKey(nameof(MemberTo))]
        public string MemberToId { get; set; }

        public virtual User MemberTo { get; set; }
    }
}
