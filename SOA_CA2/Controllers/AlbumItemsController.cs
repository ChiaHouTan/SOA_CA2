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
    public class AlbumItemsController : ControllerBase
    {
        private readonly SingerContext _context;

        public AlbumItemsController(SingerContext context)
        {
            _context = context;
        }

        // GET: api/AlbumItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumItem>>> GetAlbumsItem()
        {
            return await _context.AlbumsItem.ToListAsync();
        }

        // GET: api/AlbumItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumItem>> GetAlbumItem(Guid id)
        {
            var albumItem = await _context.AlbumsItem.FindAsync(id);

            if (albumItem == null)
            {
                return NotFound();
            }

            return albumItem;
        }

        // PUT: api/AlbumItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbumItem(Guid id, AlbumItem albumItem)
        {
            if (id != albumItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(albumItem).State = EntityState.Modified;

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
            _context.AlbumsItem.Add(albumItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbumItem), new { id = albumItem.ID }, albumItem);
        }

        // DELETE: api/AlbumItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbumItem(Guid id)
        {
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
