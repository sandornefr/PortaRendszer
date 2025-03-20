using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;

namespace PortaRendszer.Controllers
{
    [Route("api/tanterem")]
    [ApiController]
    public class TanteremController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public TanteremController(PortarendszerContext context)
        {
            _context = context;
        }

        // 1️. Összes tanterem lekérdezése
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tanterem>>> GetTantermek()
        {
            return await _context.Tantermek.ToListAsync();
        }

        // 2️. Egy adott tanterem lekérdezése ID alapján
        [HttpGet("{id}")]
        public async Task<ActionResult<Tanterem>> GetTanterem(int id)
        {
            var tanterem = await _context.Tantermek.FindAsync(id);
            if (tanterem == null)
            {
                return NotFound();
            }
            return tanterem;
        }

        // 3️. Új tanterem létrehozása
        [HttpPost]
        public async Task<ActionResult<Tanterem>> PostTanterem(Tanterem tanterem)
        {
            _context.Tantermek.Add(tanterem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTanterem), new { id = tanterem.Id }, tanterem);
        }

        // 4️. Egy adott tanterem használati naplóinak lekérdezése
        [HttpGet("{id}/hasznalat")]
        public async Task<ActionResult<IEnumerable<TanteremHasznalat>>> GetTanteremHasznalat(int id)
        {
            var hasznalatok = await _context.TanteremHasznalatok
                .Where(th => th.TanteremId == id)
                .ToListAsync();

            if (hasznalatok == null || !hasznalatok.Any())
            {
                return NotFound();
            }
            return hasznalatok;
        }

        // 5️. Új tanterem használati bejegyzés létrehozása
        [HttpPost("{id}/hasznalat")]
        public async Task<ActionResult<TanteremHasznalat>> PostTanteremHasznalat(int id, TanteremHasznalat hasznalat)
        {
            var tanterem = await _context.Tantermek.FindAsync(id);
            if (tanterem == null)
            {
                return NotFound("A tanterem nem található.");
            }

            hasznalat.TanteremId = id;
            _context.TanteremHasznalatok.Add(hasznalat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTanteremHasznalat), new { id = hasznalat.Id }, hasznalat);
        }
    }
}
