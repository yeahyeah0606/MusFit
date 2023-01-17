using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusFit.Models;

namespace MusFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly MusFitContext _context;

        public ClassesController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            return await _context.Classes.ToListAsync();
        }

        [HttpGet("operationPercentage/")]
        public IActionResult GetOperationPercentage()
        {
            var actualTotal = _context.Classes.Sum(x => x.CActual);
            var expectTotal = _context.Classes.Sum(x => x.CExpect);

            double avg = (double)actualTotal / expectTotal;
            double operationPercentage = avg * 100.0;
            return Ok(operationPercentage);
        }

        [HttpGet("signedNumber/")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetSignedNumber()
        {
            var actualNumber = from c in _context.Classes
                               join ct in _context.ClassTimes on c.CId equals ct.CId
                               join l in _context.LessionCategories on c.LcId equals l.LcId
                               where ct.CtDate > DateTime.UtcNow && ct.CtLession == 1
                               select new
                               {
                                   cID = c.CId,
                                   classNumber = c.CNumber,
                                   className = c.CName,
                                   actualNumber = c.CActual,
                                   color = l.LcThemeColor,
                                   date = ct.CtDate
                               };

            return await actualNumber.ToListAsync();
        }



        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var @class = await _context.Classes.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return @class;
        }

        // PUT: api/Classes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, Class @class)
        {
            if (id != @class.CId)
            {
                return BadRequest();
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/Classes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class @class)
        {
            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = @class.CId }, @class);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.CId == id);
        }
    }
}
