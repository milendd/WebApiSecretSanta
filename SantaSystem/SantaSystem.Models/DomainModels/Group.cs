using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SantaSystem.Models.DomainModels
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(200)]
        [Index("IX_Group_Name", IsUnique = true)]
        public string Name { get; set; }

        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }
    }
}
