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
    public class OsztalyController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public OsztalyController(PortarendszerContext context)
        {
            _context = context;
        }

        // GET: api/Osztalyok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Osztaly>>> GetOsztalyok()
        {
            return await _context.Osztalyok.ToListAsync();
        }

        // GET: api/Osztalyok/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Osztaly>> GetOsztaly(int id)
        {
            var osztaly = await _context.Osztalyok.FindAsync(id);

            if (osztaly == null)
            {
                return NotFound();
            }

            return osztaly;
        }

        // PUT: api/Osztalyok/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOsztaly(int id, Osztaly osztaly)
        {
            if (id != osztaly.Id)
            {
                return BadRequest();
            }

            _context.Entry(osztaly).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OsztalyExists(id))
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

        // POST: api/Osztalyok
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Osztaly>> PostOsztaly(Osztaly osztaly)
        {
            _context.Osztalyok.Add(osztaly);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOsztaly", new { id = osztaly.Id }, osztaly);
        }

        // DELETE: api/Osztalyok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOsztaly(int id)
        {
            var osztaly = await _context.Osztalyok.FindAsync(id);
            if (osztaly == null)
            {
                return NotFound();
            }

            _context.Osztalyok.Remove(osztaly);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OsztalyExists(int id)
        {
            return _context.Osztalyok.Any(e => e.Id == id);
        }
    }
}
