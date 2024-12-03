using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Newtonsoft.Json;

namespace SOA_CA2.Models
{
    public class SingerItem
    {
        public enum gender
        {
            male,
            female,
            unknown
        }
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string SingerName { get; set; }
        public int SingerAge { get; set; }
        [Required]
        [JsonConverter(typeof(EnumStringConverter))]
        public gender SingerGender { get; set; }
        public DateTime YearOfDebut { get; set; }
        public ICollection<AlbumItem> Albums { get; set; } = new List<AlbumItem>();
    }

    public class SingerDto
    {

        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string SingerName { get; set; }
        public int SingerAge { get; set; }
        public string SingerGender { get; set; }
        public string YearOfDebut { get; set; }
        public ICollection<AlbumDto> Albums { get; set; } = new List<AlbumDto>();
    }
}
