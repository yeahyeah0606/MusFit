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
    public class ClassRecordsController : ControllerBase
    {
        private readonly MusFitContext _context;

        public ClassRecordsController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/ClassRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassRecord>>> GetClassRecords()
        {
            return await _context.ClassRecords.ToListAsync();
        }

        // GET: api/ClassRecords/sId/classtimeID/
        [HttpGet("sId/classtimeID/{sId}/{classtimeID}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetSpecificRecords(int sId, int classtimeID)
        {
            var classrecords = from cr in _context.ClassRecords
                         where cr.SId == sId && cr.ClassTimeId== classtimeID
                         select cr;
            return await classrecords.ToListAsync();
        }


        // GET: api/ClassRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassRecord>> GetClassRecord(int id)
        {
            var classRecord = await _context.ClassRecords.FindAsync(id);

            if (classRecord == null)
            {
                return NotFound();
            }

            return classRecord;
        }

        // PUT: api/ClassRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassRecord(int id, ClassRecord classRecord)
        {
            if (id != classRecord.CrId)
            {
                return BadRequest();
            }

            _context.Entry(classRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassRecordExists(id))
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

        // POST: api/ClassRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassRecord>> PostClassRecord(ClassRecord classRecord)
        {
            _context.ClassRecords.Add(classRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassRecord", new { id = classRecord.CrId }, classRecord);
        }

        // DELETE: api/ClassRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassRecord(int id)
        {
            var classRecord = await _context.ClassRecords.FindAsync(id);
            if (classRecord == null)
            {
                return NotFound();
            }

            _context.ClassRecords.Remove(classRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassRecordExists(int id)
        {
            return _context.ClassRecords.Any(e => e.CrId == id);
        }
    }
}
