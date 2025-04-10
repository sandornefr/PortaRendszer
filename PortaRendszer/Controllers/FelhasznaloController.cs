using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.DTOs;
using PortaRendszer.Models;
using PortaRendszer.Services;
using System.Security.Cryptography;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FelhasznaloDTO>>> GetFelhasznalok()
        {
            var felhasznalok = await _context.Felhasznalos
                .Include(f => f.Belepes)
                .Include(f => f.Osztalies)
                .ToListAsync();

            return felhasznalok.Select(FelhasznaloDTO.FromEntity).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznalo(int id)
        {
            var f = await _context.Felhasznalos
                .Include(f => f.Belepes)
                .Include(f => f.Osztalies)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (f == null) return NotFound();

            return FelhasznaloDTO.FromEntity(f);
        }

        [HttpPost("regisztral")]
        public async Task<IActionResult> Regisztral(FelhasznaloCreateDto dto)
        {
            byte[] hash, salt;
            PasswordService.CreatePasswordHash(dto.Jelszo, out hash, out salt);

            var felhasznalo = new Felhasznalo
            {
                Nev = dto.Nev,
                Felhasznalonev = dto.Felhasznalonev,
                Email = dto.Email,
                Beosztas = dto.Beosztas,
                JelszoHash = hash,
                JelszoSalt = salt
            };

            _context.Felhasznalos.Add(felhasznalo);
            await _context.SaveChangesAsync();

            return Ok("Felhasználó létrehozva.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFelhasznalo(int id, FelhasznaloDTO dto)
        {
            var f = await _context.Felhasznalos.FindAsync(id);
            if (f == null) return NotFound();

            f.Nev = dto.Nev;
            f.Beosztas = dto.Beosztas;
            f.Felhasznalonev = dto.Felhasznalonev;
            f.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFelhasznalo(int id)
        {
            var f = await _context.Felhasznalos.FindAsync(id);
            if (f == null) return NotFound();

            _context.Felhasznalos.Remove(f);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "osztalyfonok,admin")]
        [HttpGet("csak-osztalyfonoknek")]
        public IActionResult GetAdatOsztalyfonoknek()
        {
            return Ok("Ezt csak az osztályfőnök láthatja.");
        }

        [Authorize]
        [HttpPost("jelszocsere")]
        public async Task<IActionResult> JelszoCsere([FromBody] JelszoCsereDTO dto)
        {
            var bejelentkezettFelhasznalo = User.Identity?.Name;

            if (string.IsNullOrEmpty(bejelentkezettFelhasznalo))
                return Unauthorized("Nincs bejelentkezve felhasználó.");

            var felhasznalo = await _context.Felhasznalos.FirstOrDefaultAsync(f => f.Felhasznalonev == bejelentkezettFelhasznalo);

            if (felhasznalo == null)
                return NotFound("A felhasználó nem található.");

            if (!PasswordService.VerifyPasswordHash(dto.JelenlegiJelszo, felhasznalo.JelszoHash, felhasznalo.JelszoSalt))
                return BadRequest("A megadott jelenlegi jelszó hibás.");

            byte[] ujHash, ujSalt;
            PasswordService.CreatePasswordHash(dto.UjJelszo, out ujHash, out ujSalt);
            felhasznalo.JelszoHash = ujHash;
            felhasznalo.JelszoSalt = ujSalt;

            await _context.SaveChangesAsync();
            return Ok("A jelszót sikeresen módosítottuk.");
        }

        [Authorize]
        [HttpGet("aktualis")]
        public async Task<ActionResult<FelhasznaloDTO>> GetAktualisFelhasznalo()
        {
            var felhasznalonev = User.Identity?.Name;

            if (string.IsNullOrEmpty(felhasznalonev))
                return Unauthorized("Nem sikerült azonosítani a bejelentkezett felhasználót.");

            var felhasznalo = await _context.Felhasznalos
                .Include(f => f.Osztalies)
                .Include(f => f.Belepes)
                .FirstOrDefaultAsync(f => f.Felhasznalonev == felhasznalonev);

            if (felhasznalo == null)
                return NotFound("A felhasználó nem található.");

            return FelhasznaloDTO.FromEntity(felhasznalo);
        }

        public static (string hash, string salt) GeneratePasswordHash(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
            var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
            return (hash, salt);
        }
    }
}
