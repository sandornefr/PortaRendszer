using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;

[Route("api/[controller]")]
[ApiController]
public class TanuloArchivController : ControllerBase
{
    private readonly PortarendszerContext _context;

    public TanuloArchivController(PortarendszerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TanuloArchiv>>> GetArchivaltTanulok()
    {
        return await _context.TanuloArchiv.ToListAsync();
    }

    [HttpPost("{tanuloId}")]
    public async Task<IActionResult> ArchivbaTesz(int tanuloId)
    {
        var tanulo = await _context.Tanulos
            .Include(t => t.Osztaly)
            .FirstOrDefaultAsync(t => t.Id == tanuloId);

        if (tanulo == null)
            return NotFound("A tanuló nem található.");

        var archiv = new TanuloArchiv
        {
            OktAzonosito = tanulo.OktAzonosito,
            Nev = tanulo.Nev,
            OsztalyNev = tanulo.Osztaly?.Nev,
            TorlesIdopont = DateTime.Now
        };

        _context.TanuloArchiv.Add(archiv);
        _context.Tanulos.Remove(tanulo);

        await _context.SaveChangesAsync();

        return Ok("Tanuló archiválva és törölve.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> VeglegesTorles(int id)
    {
        var archiv = await _context.TanuloArchiv.FindAsync(id);
        if (archiv == null) return NotFound();

        _context.TanuloArchiv.Remove(archiv);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
