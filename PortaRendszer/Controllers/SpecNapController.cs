using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;

namespace PortaRendszer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecNapController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public SpecNapController(PortarendszerContext context)
        {
            _context = context;
        }

        // GET: api/SpecNap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecNap>>> GetSpecNapok()
        {
            return await _context.SpecNapok.ToListAsync();
        }

        // GET: api/SpecNap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecNap>> GetSpecNap(int id)
        {
            var specNap = await _context.SpecNapok.FindAsync(id);

            if (specNap == null)
            {
                return NotFound();
            }

            return specNap;
        }

        // PUT: api/SpecNap/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecNap(int id, SpecNap specNap)
        {
            if (id != specNap.Id)
            {
                return BadRequest();
            }

            _context.Entry(specNap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecNapExists(id))
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

        // POST: api/SpecNap
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SpecNap>> PostSpecNap(SpecNap specNap)
        {
            _context.SpecNapok.Add(specNap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecNap", new { id = specNap.Id }, specNap);
        }

        // DELETE: api/SpecNap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecNap(int id)
        {
            var specNap = await _context.SpecNapok.FindAsync(id);
            if (specNap == null)
            {
                return NotFound();
            }

            _context.SpecNapok.Remove(specNap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecNapExists(int id)
        {
            return _context.SpecNapok.Any(e => e.Id == id);
        }
    }
}
