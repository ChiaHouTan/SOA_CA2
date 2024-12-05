using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongItemsController : ControllerBase
    {
        private readonly SingerContext _context;

        public SongItemsController(SingerContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/SongItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsItem()
        {
            if (_context.SongsItem == null)
            {
                return NotFound();
            }

            return await _context.SongsItem.Select(song =>
                new SongDto()
                {
                    ID = song.ID,
                    SongName = song.SongName,
                    SongDuration = song.SongDuration,
                    Lyricist = song.Lyricist,
                    Composer = song.Composer,
                    Arranger = song.Arranger,
                    SongURL = song.SongURL,
                    AlbumID = song.AlbumID
                }
                ).ToListAsync();
        }

        // GET: api/SongItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDto>> GetSongItem(Guid id)
        {
            if (_context.SongsItem == null)
            {
                return NotFound();
            }
            // var songItem = await _context.SongsItem.FindAsync(id);
            var songItem = await _context.SongsItem
                   .Where(song => song.ID == id)
                   .Select(song => new SongDto()
                   {
                       ID = song.ID,
                       SongName = song.SongName,
                       SongDuration = song.SongDuration,
                       Lyricist = song.Lyricist,
                       Composer = song.Composer,
                       Arranger = song.Arranger,
                       SongURL = song.SongURL,
                       AlbumID = song.AlbumID
                   })
                   .FirstOrDefaultAsync();

            if (songItem == null)
            {
                return NotFound();
            }

            return songItem;
        }

        // PUT: api/SongItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSongItem(Guid id, SongItem song)
        {
            var userRole = HttpContext.Items["UserRole"]?.ToString();
            if (userRole != "Admin")
            {
                return Forbid("Only admins can PUT.");
            }

            if (id != song.ID)
            {
                return BadRequest();
            }

            // Check if the associated singer exists
            var songItem = await _context.SongsItem.FindAsync(id);
            if (songItem == null)
            {
                return NotFound();
            }

            // Update only the specified fields
            songItem.SongName = song.SongName;
            songItem.SongDuration = song.SongDuration;
            songItem.Lyricist = song.Lyricist;
            songItem.Composer = song.Composer;
            songItem.Arranger = song.Arranger;
            songItem.SongURL = song.SongURL;
            songItem.AlbumID = song.AlbumID;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongItemExists(id))
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

        // POST: api/SongItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SongItem>> PostSongItem(SongItem songItem)
        {
            var userRole = HttpContext.Items["UserRole"]?.ToString();
            if (userRole != "Admin")
            {
                return Forbid("Only admins can POST.");
            }

            if (_context.SongsItem == null)
            {
                return Problem("Entity set 'SingerContext.SongsItem'  is null.");
            }

            _context.SongsItem.Add(songItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSongItem), new { id = songItem.ID }, songItem);
        }

        // DELETE: api/SongItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSongItem(Guid id)
        {
            var userRole = HttpContext.Items["UserRole"]?.ToString();
            if (userRole != "Admin")
            {
                return Forbid("Only admins can DELETE.");
            }

            if (_context.SongsItem == null)
            {
                return NotFound();
            }
            var songItem = await _context.SongsItem.FindAsync(id);
            if (songItem == null)
            {
                return NotFound();
            }

            _context.SongsItem.Remove(songItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongItemExists(Guid id)
        {
            return _context.SongsItem.Any(e => e.ID == id);
        }
    }
}
