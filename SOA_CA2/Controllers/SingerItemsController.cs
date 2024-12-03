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
        }

        // GET: api/SingerItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SingerItem>>> GetSingersItem()
        {
            return await _context.SingersItem.ToListAsync();
        }

        // GET: api/SingerItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SingerItem>> GetSingerItem(Guid id)
        {
            var singerItem = await _context.SingersItem.FindAsync(id);

            if (singerItem == null)
            {
                return NotFound();
            }

            return singerItem;
        }

        // PUT: api/SingerItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSingerItem(Guid id, SingerItem singerItem)
        {
            if (id != singerItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(singerItem).State = EntityState.Modified;

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
            _context.SingersItem.Add(singerItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingerItem), new { id = singerItem.ID }, singerItem);
        }

        // DELETE: api/SingerItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSingerItem(Guid id)
        {
            var singerItem = await _context.SingersItem.FindAsync(id);
            if (singerItem == null)
            {
                return NotFound();
            }

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
