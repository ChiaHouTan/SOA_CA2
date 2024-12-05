using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumItemsController : ControllerBase
    {
        private readonly SingerContext _context;

        public AlbumItemsController(SingerContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/AlbumItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsItem()
        {
            if (_context.AlbumsItem == null)
            {
                return NotFound();
            }

            return await _context.AlbumsItem
            .Include(album => album.Songs) // Include Songs for the album
       .Select(album => new AlbumDto
       {
           ID = album.ID,
           AlbumName = album.AlbumName,
           ReleaseDate = album.ReleaseDate.ToString("yyyy-MM-dd"),
           AlbumCover = album.AlbumCover,
           SingerID = album.SingerID,
           Songs = album.Songs.Select(song => new SongDto
           {
               ID = song.ID,
               SongName = song.SongName,
               SongDuration = song.SongDuration,
               Lyricist = song.Lyricist,
               Composer = song.Composer,
               Arranger = song.Arranger,
               SongURL = song.SongURL,
               AlbumID = song.AlbumID
           }).ToList()
       })
       .ToListAsync();
        }

        // GET: api/AlbumItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumDto>> GetAlbumItem(Guid id)
        {
            if (_context.AlbumsItem == null)
            {
                return NotFound();
            }

            var albumItem = await _context.AlbumsItem
                .Where(album => album.ID == id)
                .Include(album => album.Songs) // Include Songs for the album
       .Select(album => new AlbumDto
       {
           ID = album.ID,
           AlbumName = album.AlbumName,
           ReleaseDate = album.ReleaseDate.ToString("yyyy-MM-dd"),
           AlbumCover = album.AlbumCover,
           SingerID = album.SingerID,
           Songs = album.Songs.Select(song => new SongDto
           {
               ID = song.ID,
               SongName = song.SongName,
               SongDuration = song.SongDuration,
               Lyricist = song.Lyricist,
               Composer = song.Composer,
               Arranger = song.Arranger,
               SongURL = song.SongURL,
               AlbumID = song.AlbumID
           }).ToList()
       })
        .FirstOrDefaultAsync();

            if (albumItem == null)
            {
                return NotFound();
            }

            return albumItem;
        }

        // PUT: api/AlbumItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbumItem(Guid id, AlbumItem album)
        {
            var userRole = HttpContext.Items["UserRole"]?.ToString();
            if (userRole != "Admin")
            {
                return Forbid("Only admins can PUT.");
            }

            if (id != album.ID)
            {
                return BadRequest();
            }

            // Check if the associated singer exists
            var albumItem = await _context.AlbumsItem.FindAsync(id);
            if (albumItem == null)
            {
                return NotFound();
            }

            // Update only the specified fields
            albumItem.AlbumName = album.AlbumName;
            albumItem.ReleaseDate = album.ReleaseDate;
            albumItem.AlbumCover = album.AlbumCover;
            albumItem.SingerID = album.SingerID;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AlbumItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AlbumItem>> PostAlbumItem(AlbumItem albumItem)
        {
            var userRole = HttpContext.Items["UserRole"]?.ToString();
            if (userRole != "Admin")
            {
                return Forbid("Only admins can POST.");
            }

            if (_context.AlbumsItem == null)
            {
                return Problem("Entity set 'SingerContext.AlbumsItem'  is null.");
            }

            _context.AlbumsItem.Add(albumItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbumItem), new { id = albumItem.ID }, albumItem);
        }

        // DELETE: api/AlbumItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbumItem(Guid id)
        {
            var userRole = HttpContext.Items["UserRole"]?.ToString();
            if (userRole != "Admin")
            {
                return Forbid("Only admins can DELETE.");
            }

            if (_context.AlbumsItem == null)
            {
                return NotFound();
            }
            var albumItem = await _context.AlbumsItem.FindAsync(id);
            if (albumItem == null)
            {
                return NotFound();
            }

            _context.AlbumsItem.Remove(albumItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumItemExists(Guid id)
        {
            return _context.AlbumsItem.Any(e => e.ID == id);
        }
    }
}
