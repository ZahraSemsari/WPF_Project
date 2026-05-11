using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    public class TODOTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Starred { get; set; }
        public bool Checked { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [ForeignKey("TODOList")]
        public int ListId { get; set; } 
    }

    public class TODOList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
