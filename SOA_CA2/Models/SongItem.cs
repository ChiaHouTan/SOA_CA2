using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models
{
    public class SongItem
    {
        [Key]
        public Guid ID { get; set; }
        public string SongName { get; set; }
        public double SongDuration { get; set; }
        public string? Lyricist { get; set; }
        public string? Composer { get; set; }
        public string? Arranger { get; set; }
        public string SongURL { get; set; }
        [ForeignKey("AlbumID")]
        public AlbumItem? Album { get; set; }
        public Guid AlbumID { get; set; }
    }

    public class SongDto
    {
        public Guid ID { get; set; }
        public string SongName { get; set; }
        public double SongDuration { get; set; }
        public string? Lyricist { get; set; }
        public string? Composer { get; set; }
        public string? Arranger { get; set; }
        public string SongURL { get; set; }
        public Guid AlbumID { get; set; }
    }
}
