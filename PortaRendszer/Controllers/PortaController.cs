using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;

namespace PortaRendszer.Controllers
{
    [Route("api/porta")]
    [ApiController]
    public class PortaController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public PortaController(PortarendszerContext context)
        {
            _context = context;
        }

        // 1️. Összes porta üzenet lekérdezése
        [HttpGet("uzenetek")]
        public async Task<ActionResult<IEnumerable<PortaUzenet>>> GetPortaUzenetek()
        {
            return await _context.PortaUzenetek.ToListAsync();
        }

        // 2️. Egy adott porta üzenet lekérdezése ID alapján
        [HttpGet("uzenet/{id}")]
        public async Task<ActionResult<PortaUzenet>> GetPortaUzenet(int id)
        {
            var uzenet = await _context.PortaUzenetek.FindAsync(id);
            if (uzenet == null)
            {
                return NotFound();
            }
            return uzenet;
        }

        // 3️. Új porta üzenet létrehozása
        [HttpPost("uzenet")]
        public async Task<ActionResult<PortaUzenet>> PostPortaUzenet(PortaUzenet uzenet)
        {
            _context.PortaUzenetek.Add(uzenet);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPortaUzenet), new { id = uzenet.Id }, uzenet);
        }

        // 4️. Egy adott tanulóhoz tartozó porta üzenetek lekérdezése
        [HttpGet("tanulo/{id}/uzenetek")]
        public async Task<ActionResult<IEnumerable<PortaUzenet>>> GetTanuloPortaUzenetek(int id)
        {
            var uzenetek = await _context.PortaUzenetek
                .Where(u => u.TanuloId == id)
                .ToListAsync();

            if (!uzenetek.Any())
            {
                return NotFound("Nincsenek porta üzenetek ehhez a tanulóhoz.");
            }
            return uzenetek;
        }

        // 5️. Egy porta üzenet törlése
        [HttpDelete("uzenet/{id}")]
        public async Task<IActionResult> DeletePortaUzenet(int id)
        {
            var uzenet = await _context.PortaUzenetek.FindAsync(id);
            if (uzenet == null)
            {
                return NotFound();
            }

            _context.PortaUzenetek.Remove(uzenet);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
