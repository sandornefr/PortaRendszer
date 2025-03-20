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
    [Route("api/[controller]")]
    [ApiController]
    public class PortaUzenetController : ControllerBase
    {
        private readonly PortarendszerContext _context;

        public PortaUzenetController(PortarendszerContext context)
        {
            _context = context;
        }

        // GET: api/PortaUzenetek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortaUzenet>>> GetPortaUzenetek()
        {
            return await _context.PortaUzenetek.ToListAsync();
        }

        // GET: api/PortaUzenetek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PortaUzenet>> GetPortaUzenet(int id)
        {
            var portaUzenet = await _context.PortaUzenetek.FindAsync(id);

            if (portaUzenet == null)
            {
                return NotFound();
            }

            return portaUzenet;
        }

        // PUT: api/PortaUzenetek/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPortaUzenet(int id, PortaUzenet portaUzenet)
        {
            if (id != portaUzenet.Id)
            {
                return BadRequest();
            }

            _context.Entry(portaUzenet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortaUzenetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PortaUzenetek
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PortaUzenet>> PostPortaUzenet(PortaUzenet portaUzenet)
        {
            _context.PortaUzenetek.Add(portaUzenet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPortaUzenet", new { id = portaUzenet.Id }, portaUzenet);
        }

        // DELETE: api/PortaUzenetek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortaUzenet(int id)
        {
            var portaUzenet = await _context.PortaUzenetek.FindAsync(id);
            if (portaUzenet == null)
            {
                return NotFound();
            }

            _context.PortaUzenetek.Remove(portaUzenet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PortaUzenetExists(int id)
        {
            return _context.PortaUzenetek.Any(e => e.Id == id);
        }
    }
}
