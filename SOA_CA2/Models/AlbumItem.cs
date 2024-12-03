using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models
{
    public class AlbumItem
    {
        [Key]
        public Guid ID { get; set; }
        public string AlbumName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? AlbumCover { get; set; }
        public ICollection<SongItem> Songs { get; set; } = new List<SongItem>();

        [ForeignKey("SingerID")]
        public SingerItem Singer { get; set; }
        public Guid SingerID { get; set; }
    }
}
