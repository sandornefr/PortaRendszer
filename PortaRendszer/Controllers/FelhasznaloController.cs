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
    [Route("api/felhasznalo")]
    [ApiController]
    public class FelhasznaloController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public FelhasznaloController(PortarendszerContext context)
        {
            _context = context;
        }

        // 1️. Összes felhasználó lekérdezése
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Felhasznalo>>> GetFelhasznalok()
        {
            return await _context.Felhasznalok.ToListAsync();
        }

        // 2️. Egy adott felhasználó lekérdezése ID alapján
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

        // 3️. Új felhasználó létrehozása
        [HttpPost]
        public async Task<ActionResult<Felhasznalo>> PostFelhasznalo(Felhasznalo felhasznalo)
        {
            _context.Felhasznalok.Add(felhasznalo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felhasznalo.Id }, felhasznalo);
        }

        // 4️. Egy adott felhasználóhoz tartozó belépések lekérdezése
        [HttpGet("{id}/belepesek")]
        public async Task<ActionResult<IEnumerable<Belepes>>> GetFelhasznaloBelepesek(int id)
        {
            var belepesek = await _context.Belepesek
                .Where(b => b.FelhasznaloId == id)
                .ToListAsync();

            if (!belepesek.Any())
            {
                return NotFound("Nincs belépési adat ehhez a felhasználóhoz.");
            }
            return belepesek;
        }

        // 5️. Új belépési napló bejegyzés létrehozása
        [HttpPost("{id}/belepes")]
        public async Task<ActionResult<Belepes>> PostFelhasznaloBelepes(int id)
        {
            var felhasznalo = await _context.Felhasznalok.FindAsync(id);
            if (felhasznalo == null)
            {
                return NotFound("A felhasználó nem található.");
            }

            var belepes = new Belepes
            {
                FelhasznaloId = id,
                BelepesIdo = DateTime.UtcNow,
                UtolsoAktivitas = DateTime.UtcNow
            };

            _context.Belepesek.Add(belepes);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFelhasznaloBelepesek), new { id = belepes.Id }, belepes);
        }

        // 6️. Kijelentkezés rögzítése (kilepesi_ido beállítása)
        [HttpPut("{felhasznaloId}/belepes/{belepesId}/kilepes")]
        public async Task<IActionResult> PutKilepes(int felhasznaloId, int belepesId)
        {
            var belepes = await _context.Belepesek
                .FirstOrDefaultAsync(b => b.Id == belepesId && b.FelhasznaloId == felhasznaloId);

            if (belepes == null)
            {
                return NotFound("A belépési rekord nem található.");
            }

            belepes.KilepesiIdo = DateTime.UtcNow;
            belepes.UtolsoAktivitas = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
