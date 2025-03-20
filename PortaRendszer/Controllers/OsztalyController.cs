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
    [Route("api/osztaly")]
    [ApiController]
    public class OsztalyController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public OsztalyController(PortarendszerContext context)
        {
            _context = context;
        }

        // 1️. Összes osztály lekérdezése
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Osztaly>>> GetOsztalyok()
        {
            return await _context.Osztalyok.ToListAsync();
        }

        // 2️. Egy adott osztály lekérdezése ID alapján
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

        // 3️. Új osztály létrehozása
        [HttpPost]
        public async Task<ActionResult<Osztaly>> PostOsztaly(Osztaly osztaly)
        {
            _context.Osztalyok.Add(osztaly);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOsztaly), new { id = osztaly.Id }, osztaly);
        }

        // 4️. Egy adott osztályhoz tartozó tanulók lekérdezése
        [HttpGet("{id}/tanulok")]
        public async Task<ActionResult<IEnumerable<Tanulo>>> GetOsztalyTanulok(int id)
        {
            var tanulok = await _context.Tanulok
                .Where(t => t.OsztalyId == id)
                .ToListAsync();

            if (!tanulok.Any())
            {
                return NotFound("Nincsenek tanulók ebben az osztályban.");
            }
            return tanulok;
        }

        // 5️. Új tanuló hozzáadása egy adott osztályhoz
        [HttpPost("{id}/tanulo")]
        public async Task<ActionResult<Tanulo>> PostOsztalyTanulo(int id, Tanulo tanulo)
        {
            var osztaly = await _context.Osztalyok.FindAsync(id);
            if (osztaly == null)
            {
                return NotFound("Az osztály nem található.");
            }

            tanulo.OsztalyId = id; // Osztályhoz rendelés
            _context.Tanulok.Add(tanulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOsztalyTanulok), new { id = tanulo.Id }, tanulo);
        }

        // 6️. Egy adott tanuló törlése az osztályból
        [HttpDelete("{osztalyId}/tanulo/{tanuloId}")]
        public async Task<IActionResult> DeleteOsztalyTanulo(int osztalyId, int tanuloId)
        {
            var tanulo = await _context.Tanulok.FindAsync(tanuloId);
            if (tanulo == null || tanulo.OsztalyId != osztalyId)
            {
                return NotFound("A tanuló nem található az adott osztályban.");
            }

            _context.Tanulok.Remove(tanulo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
