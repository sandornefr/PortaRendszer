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
    public class TanuloController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public TanuloController(PortarendszerContext context)
        {
            _context = context;
        }

        // GET: api/Tanulok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tanulo>>> GetTanulok()
        {
            return await _context.Tanulok.ToListAsync();
        }

        // GET: api/Tanulok/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tanulo>> GetTanulo(int id)
        {
            var tanulo = await _context.Tanulok.FindAsync(id);

            if (tanulo == null)
            {
                return NotFound();
            }

            return tanulo;
        }

        // PUT: api/Tanulok/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTanulo(int id, Tanulo tanulo)
        {
            if (id != tanulo.Id)
            {
                return BadRequest();
            }

            _context.Entry(tanulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TanuloExists(id))
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

        // POST: api/Tanulok
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tanulo>> PostTanulo(Tanulo tanulo)
        {
            _context.Tanulok.Add(tanulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTanulo", new { id = tanulo.Id }, tanulo);
        }

        // DELETE: api/Tanulok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTanulo(int id)
        {
            var tanulo = await _context.Tanulok.FindAsync(id);
            if (tanulo == null)
            {
                return NotFound();
            }

            _context.Tanulok.Remove(tanulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TanuloExists(int id)
        {
            return _context.Tanulok.Any(e => e.Id == id);
        }
    }
}
