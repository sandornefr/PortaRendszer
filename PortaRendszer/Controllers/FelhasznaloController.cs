using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.DTOs;
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
        public async Task<ActionResult<IEnumerable<FelhasznaloDTO>>> GetFelhasznalok()
        {
            var felhasznalok = await _context.Felhasznalos
                .Include(f => f.Osztalies)
                .Include(f => f.Belepes)
                .ToListAsync();

            var dtos = felhasznalok.Select(FelhasznaloDTO.FromEntity).ToList();
            return Ok(dtos);
        }

        // GET: api/Felhasznalo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznalo(int id)
        {
            var f = await _context.Felhasznalos
                .Include(f => f.Osztalies)
                .Include(f => f.Belepes)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (f == null) return NotFound();

            return FelhasznaloDTO.FromEntity(f);
        }

        // POST: api/Felhasznalo
        [HttpPost]
        public async Task<ActionResult<FelhasznaloDTO>> PostFelhasznalo(FelhasznaloDTO dto)
        {
            var f = new Felhasznalo
            {
                Nev = dto.Nev,
                Beosztas = dto.Beosztas,
                Felhasznalonev = dto.Felhasznalonev,
                Email = dto.Email,
                Jelszo = "Titkos123" // ideiglenes jelszó, majd cserélhető
            };

            _context.Felhasznalos.Add(f);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = f.Id }, FelhasznaloDTO.FromEntity(f));
        }

        // PUT: api/Felhasznalo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFelhasznalo(int id, FelhasznaloDTO dto)
        {
            var f = await _context.Felhasznalos.FirstOrDefaultAsync(x => x.Id == id);
            if (f == null) return NotFound();

            f.Nev = dto.Nev;
            f.Beosztas = dto.Beosztas;
            f.Felhasznalonev = dto.Felhasznalonev;
            f.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Felhasznalo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFelhasznalo(int id)
        {
            var f = await _context.Felhasznalos.FindAsync(id);
            if (f == null) return NotFound();

            _context.Felhasznalos.Remove(f);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FelhasznaloExists(int id)
        {
            return _context.Felhasznalos.Any(e => e.Id == id);
        }
    }
}
