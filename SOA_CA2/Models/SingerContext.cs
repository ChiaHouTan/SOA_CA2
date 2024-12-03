using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models
{
    public class SingerContext : DbContext
    {
        public DbSet<SongItem> SongsItem { get; set; } = null;
        public DbSet<SingerItem> SingersItem { get; set; } = null;
        public DbSet<AlbumItem> AlbumsItem { get; set; } = null;

        public SingerContext(DbContextOptions<SingerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SingerItem>().HasData(
            new SingerItem()
            {
                ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                SingerName = "LiSA",
                SingerAge = 32,
                SingerGender = SingerItem.gender.female,
                YearOfDebut = new DateTime(2010, 1, 1)
            },
            new SingerItem()
            {
                ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                SingerName = "Bump of Chicken",
                SingerAge = 44,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = new DateTime(1994, 1, 1)
            },
            new SingerItem()
            {
                ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
                SingerName = "My First Story",
                SingerAge = 29,
                SingerGender = SingerItem.gender.male,
                YearOfDebut = new DateTime(2011, 1, 1)
            }
            );

            modelBuilder.Entity<AlbumItem>().HasData(
            new AlbumItem()
            {
                ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6"),
                AlbumName = "LiSA_Album",
                ReleaseDate = new DateTime(2010, 1, 1),
                AlbumCover = "Lisa_Cover",
                SingerID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            },
            new AlbumItem()
            {
                ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc7"),
                AlbumName = "Bump of Chicken_Album",
                ReleaseDate = new DateTime(1994, 1, 1),
                AlbumCover = "Bump of Chicken_Cover",
                SingerID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7")
            },
            new AlbumItem()
            {
                ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc8"),
                AlbumName = "My First Story",
                ReleaseDate = new DateTime(2011, 1, 1),
                AlbumCover = "My First Story_Cover",
                SingerID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
            }
            );

            modelBuilder.Entity<SongItem>().HasData(
                new SongItem()
                {
                    ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb5"),
                    SongName = "oath sign",
                    SongDuration = 4.09,
                    Lyricist = "Shō Watanabe",
                    Composer = "Shō Watanabe",
                    Arranger = "toku",
                    SongURL = "https://www.youtube.com/watch?v=VJZqQ-nH8qo&ab_channel=LiSAOfficialYouTube",
                    AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6")
                },
               new SongItem()
               {
                   ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb6"),
                   SongName = "Catch the Moment",
                   SongDuration = 4.42,
                   Lyricist = "LiSA",
                   Composer = "Tomoya Tabuchi",
                   Arranger = "la la larks",
                   SongURL = "https://www.youtube.com/watch?v=K1PltwBuDKM&ab_channel=LiSAOfficialYouTube",
                   AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6")
               },
                new SongItem()
                {
                    ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb7"),
                    SongName = "REALiZE",
                    SongDuration = 3.13,
                    Lyricist = "LiSA",
                    Composer = "kemu",
                    Arranger = "kemu",
                    SongURL = "https://www.youtube.com/watch?v=-A0ICz6rVvU&ab_channel=LiSAOfficialYouTube",
                    AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc6")
                },
                new SongItem()
                {
                    ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb8"),
                    SongName = "Acacia",
                    SongDuration = 4.22,
                    Lyricist = "Motoo Fujiwara",
                    Composer = "Motoo Fujiwara",
                    Arranger = "BUMP OF CHICKEN & MOR",
                    SongURL = "https://www.youtube.com/watch?v=Oh57YxEW_jQ&ab_channel=BUMPOFCHICKEN",
                    AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc7")
                },
                new SongItem()
                {
                    ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb9"),
                    SongName = "I'm a mess",
                    SongDuration = 4.09,
                    Lyricist = "MY FIRST STORY",
                    Composer = "MY FIRST STORY",
                    Arranger = "MY FIRST STORY",
                    SongURL = "https://www.youtube.com/watch?v=ma4lm5eAdvg&ab_channel=MYFIRSTSTORYOfficialYouTubeChannel",
                    AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc8")
                },
                new SongItem()
                {
                    ID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1"),
                    SongName = "i don't wanna be",
                    SongDuration = 3.36,
                    Lyricist = "MY FIRST STORY",
                    Composer = "MY FIRST STORY",
                    Arranger = "MY FIRST STORY",
                    SongURL = "https://www.youtube.com/watch?v=KBCiufulfew&ab_channel=MYFIRSTSTORYOfficialYouTubeChannel",
                    AlbumID = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afc8")
                }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
