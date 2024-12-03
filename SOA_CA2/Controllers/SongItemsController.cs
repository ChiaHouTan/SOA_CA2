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
        public async Task<IActionResult> PutSongItem(Guid id, SongItem songItem)
        {
            if (id != songItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(songItem).State = EntityState.Modified;

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
            _context.SongsItem.Add(songItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSongItem), new { id = songItem.ID }, songItem);
        }

        // DELETE: api/SongItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSongItem(Guid id)
        {
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
