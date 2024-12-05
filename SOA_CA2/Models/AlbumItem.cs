using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public SingerItem? Singer { get; set; }
        public Guid SingerID { get; set; }
    }

    public class AlbumDto
    {
        public Guid ID { get; set; }
        public string AlbumName { get; set; }
        public string ReleaseDate { get; set; }
        public string? AlbumCover { get; set; }
        public ICollection<SongDto> Songs { get; set; } = new List<SongDto>();
        public Guid SingerID { get; set; }
    }
}
