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
    public class SingerItemsController : ControllerBase
    {
        private readonly SingerContext _context;

        public SingerItemsController(SingerContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        // GET: api/SingerItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SingerDto>>> GetSingersItem()
        {
            if (_context.SingersItem == null)
            {
                return NotFound();
            }

            return await _context.SingersItem
       .Include(singer => singer.Albums)
       .ThenInclude(album => album.Songs)
       .Select(singer => new SingerDto
       {
           ID = singer.ID,
           SingerName = singer.SingerName,
           SingerAge = singer.SingerAge,
           SingerGender = singer.SingerGender.ToString(),
           YearOfDebut = singer.YearOfDebut.ToString("yyyy"),
           Albums = singer.Albums.Select(album => new AlbumDto
           {
               ID = album.ID,
               AlbumName = album.AlbumName,
               ReleaseDate = album.ReleaseDate.ToString("yyyy-MM-dd"),
               AlbumCover = album.AlbumCover,
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
               }).ToList(),
               SingerID = album.SingerID
           }).ToList()
       })
       .ToListAsync();
        }

        // GET: api/SingerItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SingerDto>> GetSingerItem(Guid id)
        {
            if (_context.SingersItem == null)
            {
                return NotFound();
            }
            var singerItem = await _context.SingersItem
                    .Where(singer => singer.ID == id)
                    .Include(singer => singer.Albums)
       .ThenInclude(album => album.Songs)
       .Select(singer => new SingerDto
       {
           ID = singer.ID,
           SingerName = singer.SingerName,
           SingerAge = singer.SingerAge,
           SingerGender = singer.SingerGender.ToString(),
           YearOfDebut = singer.YearOfDebut.ToString("yyyy"),
           Albums = singer.Albums.Select(album => new AlbumDto
           {
               ID = album.ID,
               AlbumName = album.AlbumName,
               ReleaseDate = album.ReleaseDate.ToString("yyyy-MM-dd"),
               AlbumCover = album.AlbumCover,
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
               }).ToList(),
               SingerID = album.SingerID
           }).ToList()
       })
       .FirstOrDefaultAsync();

            // var singerItem = await _context.SingersItem.FindAsync(id);

            if (singerItem == null)
            {
                return NotFound();
            }

            return singerItem;
        }

        // PUT: api/SingerItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSingerItem(Guid id, SingerItem singer)
        {
            if (id != singer.ID)
            {
                return BadRequest();
            }

            var singerItem = await _context.SingersItem.FindAsync(id);
            if (singerItem == null)
            {
                return NotFound();
            }

            // Update only the specified fields
            singerItem.ID = singer.ID;
            singerItem.SingerName = singer.SingerName;
            singerItem.SingerAge = singer.SingerAge;
            singerItem.SingerGender = singer.SingerGender;
            singerItem.YearOfDebut = singer.YearOfDebut;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SingerItemExists(id))
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

        // POST: api/SingerItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SingerItem>> PostSingerItem(SingerItem singerItem)
        {
            if (_context.SingersItem == null)
            {
                return Problem("Entity set 'SingerContext.SingersItem'  is null.");
            }
            _context.SingersItem.Add(singerItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingerItem), new { id = singerItem.ID }, singerItem);
        }

    //    // DELETE: api/SingerItems/5 baclup
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteSingerItem(Guid id)
    //    {
    //        if (_context.SingersItem == null)
    //        {
    //            return NotFound();
    //        }
    //        var singerItem = await _context.SingersItem.FindAsync(id);
    //        if (singerItem == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.SingersItem.Remove(singerItem);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool SingerItemExists(Guid id)
    //    {
    //        return _context.SingersItem.Any(e => e.ID == id);
    //    }
    //} 

    // DELETE: api/SingerItems/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSingerItem(Guid id)
        {
            if (_context.SingersItem == null)
            {
                return NotFound();
            }
            var singerItem = await _context.SingersItem.FindAsync(id);
            if (singerItem == null)
            {
                return NotFound();
            }

            var associatedAlbums = _context.AlbumsItem.Where(a => a.SingerID == id).ToList();
            foreach (var album in associatedAlbums)
            {
                var associatedSongs = _context.SongsItem.Where(s => s.AlbumID == album.ID).ToList();
                _context.SongsItem.RemoveRange(associatedSongs);
            }
            _context.AlbumsItem.RemoveRange(associatedAlbums);
            _context.SingersItem.Remove(singerItem);

            _context.SingersItem.Remove(singerItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SingerItemExists(Guid id)
        {
            return _context.SingersItem.Any(e => e.ID == id);
        }
    }
}
