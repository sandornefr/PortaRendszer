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

    [HttpPost]
    public async Task<IActionResult> ArchivaltTanuloHozzaadasa([FromBody] TanuloArchiv dto)
    {
        dto.TorlesIdopont = DateTime.Now;
        _context.TanuloArchiv.Add(dto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetArchivaltTanulok), new { id = dto.Id }, dto);
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
