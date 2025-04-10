using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.DTOs;
using PortaRendszer.Models;

namespace PortaRendszer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortaUzenetController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public PortaUzenetController(PortarendszerContext context)
        {
            _context = context;
        }

        // GET: api/PortaUzenet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortaUzenetDTO>>> GetUzenetek()
        {
            var uzenetek = await _context.Porta_Uzenet
                .Include(u => u.Tanulo)
                .ThenInclude(t => t.Osztaly)
                .ToListAsync();

            return uzenetek
                .Where(u => u.Tanulo != null && u.Tanulo.Osztaly != null)
                .Select(u => PortaUzenetDTO.FromEntity(u))
                .ToList();
        }

        // GET: api/PortaUzenet/statusz/Jelen_van
        [HttpGet("statusz/{statusz}")]
        public async Task<ActionResult<IEnumerable<PortaUzenetDTO>>> GetUzenetekByStatusz(string statusz)
        {
            if (!Enum.TryParse<StatuszTipus>(statusz, true, out var statuszEnum))
                return BadRequest($"Ismeretlen státusz: {statusz}");

            var uzenetek = await _context.Porta_Uzenet
                .Include(u => u.Tanulo)
                .ThenInclude(t => t.Osztaly)
                .Where(u => u.Statusz == statuszEnum && u.Tanulo != null && u.Tanulo.Osztaly != null)
                .ToListAsync();

            return uzenetek.Select(u => PortaUzenetDTO.FromEntity(u)).ToList();
        }

        
        [HttpPost]
        [Authorize(Roles = "portas,tanar,napkozis,admin")]
        public async Task<IActionResult> PostUzenet([FromBody] PortaUzenetCreateDto dto)
        {
            if (dto.Uzenet == null)
            {
                return BadRequest("The 'Uzenet' field cannot be null.");
            }

            var uzenet = new PortaUzenet
            {
                TanuloId = dto.TanuloId,
                Uzenet = dto.Uzenet ?? string.Empty,
                Statusz = dto.Statusz,
                Idopont = DateTime.Now
            };

            _context.Porta_Uzenet.Add(uzenet);
            await _context.SaveChangesAsync();

            return Ok(PortaUzenetDTO.FromEntity(uzenet));
        }

        // PUT: api/PortaUzenet/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUzenet(int id, PortaUzenetDTO dto)
        {
            var uzenet = await _context.Porta_Uzenet.FindAsync(id);
            if (uzenet == null) return NotFound();

            if (!Enum.TryParse<StatuszTipus>(dto.Statusz, true, out var parsedStatusz))
                return BadRequest("Érvénytelen státusz érték.");

            uzenet.Statusz = parsedStatusz;
            uzenet.Uzenet = dto.Uzenet;
            uzenet.TanuloId = dto.TanuloId;
            uzenet.Idopont = dto.Idopont;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PortaUzenet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUzenet(int id)
        {
            var uzenet = await _context.Porta_Uzenet.FindAsync(id);
            if (uzenet == null) return NotFound();

            _context.Porta_Uzenet.Remove(uzenet);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
