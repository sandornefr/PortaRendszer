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
    public class FelhasznaloController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public FelhasznaloController(PortarendszerContext context)
        {
            _context = context;
        }

        // GET: api/Felhasznalo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Felhasznalo>>> GetFelhasznalok()
        {
            return await _context.Felhasznalok.ToListAsync();
        }

        // GET: api/Felhasznalo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Felhasznalo>> GetFelhasznalo(int id)
        {
            var felhasznalo = await _context.Felhasznalok.FindAsync(id);

            if (felhasznalo == null)
            {
                return NotFound();
            }

            return felhasznalo;
        }

        // PUT: api/Felhasznalo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFelhasznalo(int id, Felhasznalo felhasznalo)
        {
            if (id != felhasznalo.Id)
            {
                return BadRequest();
            }

            _context.Entry(felhasznalo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FelhasznaloExists(id))
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

        // POST: api/Felhasznalok
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Felhasznalo>> PostFelhasznalo(Felhasznalo felhasznalo)
        {
            _context.Felhasznalok.Add(felhasznalo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFelhasznalo", new { id = felhasznalo.Id }, felhasznalo);
        }

        // DELETE: api/Felhasznalok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFelhasznalo(int id)
        {
            var felhasznalo = await _context.Felhasznalok.FindAsync(id);
            if (felhasznalo == null)
            {
                return NotFound();
            }

            _context.Felhasznalok.Remove(felhasznalo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FelhasznaloExists(int id)
        {
            return _context.Felhasznalok.Any(e => e.Id == id);
        }
    }
}
