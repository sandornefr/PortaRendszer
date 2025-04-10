using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.DTOs;
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

        // GET: api/Osztaly
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OsztalyDTO>>> GetOsztalyok()
        {
            var osztalyok = await _context.Osztalies
                .Include(o => o.Osztalyfonok)
                .ToListAsync();

            return osztalyok.Select(OsztalyDTO.FromEntity).ToList();
        }

        // GET: api/Osztaly/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OsztalyDTO>> GetOsztaly(int id)
        {
            var osztaly = await _context.Osztalies
                .Include(o => o.Osztalyfonok)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (osztaly == null)
            {
                return NotFound();
            }

            return OsztalyDTO.FromEntity(osztaly);
        }

        [HttpPost]
        [Authorize(Roles = "admin,igazgato,igazgatohelyettes")]
        public async Task<IActionResult> PostOsztaly(OsztalyCreateDTO dto)
        {
            // Ellenőrizzük, hogy a választott osztályfőnök létezik-e, és még nincs osztályhoz rendelve
            var osztalyfonok = await _context.Felhasznalos
                .FirstOrDefaultAsync(f => f.Id == dto.OsztalyfonokId && f.Beosztas == "tanar" && !_context.Osztalies.Any(o => o.OsztalyfonokId == f.Id));

            if (osztalyfonok == null)
                return BadRequest("A kiválasztott osztályfőnök nem elérhető vagy már van osztálya.");

            var osztaly = new Osztaly
            {
                Nev = dto.Nev,
                OsztalyfonokId = osztalyfonok.Id
            };

            _context.Osztalies.Add(osztaly);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOsztaly), new { id = osztaly.Id }, OsztalyDTO.FromEntity(osztaly));
        }

        [HttpGet("elerheto-osztalyfonokok")]
        public async Task<ActionResult<IEnumerable<FelhasznaloDTO>>> GetElérhetőOsztalyfonokok()
        {
            var tanarok = await _context.Felhasznalos
                .Where(f => f.Beosztas == "tanar" && !_context.Osztalies.Any(o => o.OsztalyfonokId == f.Id))
                .ToListAsync();

            return tanarok.Select(FelhasznaloDTO.FromEntity).ToList();
        }

        [HttpPost("leptetes")]
        //[Authorize(Roles = "igazgato,igazgatohelyettes,admin")]
        public async Task<IActionResult> Leptetes()
        {
            var tanulok = await _context.Tanulos
                .Include(t => t.Osztaly)
                .Where(t => t.AktivEvben == true)
                .ToListAsync();

            var archivLista = new List<TanuloArchiv>();
            int sikeresenLepettek = 0;

            foreach (var tanulo in tanulok)
            {
                var regiNev = tanulo.Osztaly.Nev;

                if (regiNev.StartsWith("8")) // például 8.a végzős
                {
                    archivLista.Add(new TanuloArchiv
                    {
                        OktAzonosito = tanulo.OktAzonosito,
                        Nev = tanulo.Nev,
                        OsztalyNev = tanulo.Osztaly.Nev,
                        TorlesIdopont = DateTime.Now
                    });

                    _context.Tanulos.Remove(tanulo);
                    continue;
                }

                var ujEvfolyam = (int)char.GetNumericValue(regiNev[0]) + 1;
                var betu = regiNev[1..];
                var ujOsztalyNev = $"{ujEvfolyam}{betu}";

                var celOsztaly = await _context.Osztalies
                    .FirstOrDefaultAsync(o => o.Nev == ujOsztalyNev);

                if (celOsztaly != null)
                {
                    tanulo.OsztalyId = celOsztaly.Id;
                    sikeresenLepettek++;
                }
                else
                {
                    // nincs ilyen osztály, maradjon a helyén vagy logolj
                }
            }

            if (archivLista.Any())
            {
                _context.TanuloArchiv.AddRange(archivLista);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                LeptetettTanulok = sikeresenLepettek,
                ArchivaltTanulok = archivLista.Count
            });
        }


        // PUT: api/Osztaly/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOsztaly(int id, OsztalyDTO dto)
        {
            var osztaly = await _context.Osztalies.FirstOrDefaultAsync(o => o.Id == id);
            if (osztaly == null) return NotFound();

            osztaly.Nev = dto.Nev;
            osztaly.Osztalyfonok = await _context.Felhasznalos.FirstOrDefaultAsync(f => f.Nev == dto.OsztalyfonokNev);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Osztaly/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOsztaly(int id)
        {
            var osztaly = await _context.Osztalies.FindAsync(id);
            if (osztaly == null) return NotFound();

            _context.Osztalies.Remove(osztaly);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

