using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMS.Api.Data;
using LMS.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Api.Controllers
{
    [Route("api/works")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly LMSApiContext _context;

        public WorksController(LMSApiContext context)
        {
            _context = context;
        }

        // GET: api/Works
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Work>>> GetWork()
        {
            return await _context.Works.ToListAsync();
        }

        // GET: api/Works/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Work>> GetWork(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work == null)
            {
                return NotFound();
            }

            return work;
        }

        // PUT: api/Works/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWork(int id, Work work)
        {
            if (id != work.Id)
            {
                return BadRequest();
            }

            _context.Entry(work).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkExists(id))
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

        // POST: api/Works
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Work>> PostWork(Work work)
        {
            _context.Works.Add(work);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWork", new { id = work.Id }, work);
        }

        // DELETE: api/Works/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWork(int id)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }

            _context.Works.Remove(work);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkExists(int id)
        {
            return _context.Works.Any(e => e.Id == id);
        }
    }
}
