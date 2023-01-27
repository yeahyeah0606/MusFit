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
    public class ClassTimesController : ControllerBase
    {
        private readonly MusFitContext _context;

        public ClassTimesController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/ClassTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassTime>>> GetClassTimes()
        {
            return await _context.ClassTimes.ToListAsync();
        }

        [HttpGet("cId/{id}")]
        public async Task<ActionResult<dynamic>> GetClassByCId(int id)
        {
            var obj = from ct in _context.ClassTimes
                      join t in _context.Terms on ct.TId equals t.TId
                      where ct.CId == id
                      select new
                      {
                          classTimeId = ct.ClassTimeId,
                          cId = ct.CId,
                          ctDate = ct.CtDate,
                          ctLession = ct.CtLession,
                          weekday = ct.Weekday,
                          startTime = t.TStartTime.ToString().Substring(0, 5),
                          endTime = t.TEndTime.ToString().Substring(0, 5)
                      };
            return await obj.ToListAsync();
        }

        [HttpGet("cId/date/{cId}/{date}")]
        public async Task<ActionResult<dynamic>> GetClassTimeIdByDatetime(int cId, DateTime date)
        {
            var obj = from ct in _context.ClassTimes
                      where ct.CId == cId && ct.CtDate == date
                      select ct;
            return await obj.ToListAsync();
        }


        [HttpGet("term/")]
        public async Task<ActionResult<dynamic>> GetClassTimeContainTerm(int id)
        {
            var obj = from ct in _context.ClassTimes
                      join t in _context.Terms on ct.TId equals t.TId
                      select new
                      {
                          classTimeId = ct.ClassTimeId,
                          cId = ct.CId,
                          ctDate = ct.CtDate,
                          ctLession = ct.CtLession,
                          weekday = ct.Weekday,
                          startTime = t.TStartTime.ToString().Substring(0, 5),
                          endTime = t.TEndTime.ToString().Substring(0, 5)
                      };
            return await obj.ToListAsync();
        }

        // GET: api/ClassTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassTime>> GetClassTime(int id)
        {
            var classTime = await _context.ClassTimes.FindAsync(id);

            if (classTime == null)
            {
                return NotFound();
            }

            return classTime;
        }
		//--------------------------------------------------------- 君 START ------------------------------------------------------------
		#region 單一課程最後一堂課日期
		// GET: api/ClassTimes/endDate/1
		[HttpGet("endDate/{id}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetEndDate(int id)
		{
			var obj = from c in _context.Classes
					  join ct in _context.ClassTimes on c.CId equals ct.CId
					  where ct.CtLession == c.CTotalLession & c.CId == id
					  select new
					  {
						  cID = id,
                          endDate = ct.CtDate
					  };
			return await obj.ToListAsync();
		}
		#endregion

		//---------------------------------------------------------  君 END  ------------------------------------------------------------

		// PUT: api/ClassTimes/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutClassTime(int id, ClassTime classTime)
        {
            if (id != classTime.ClassTimeId)
            {
                return BadRequest();
            }

            _context.Entry(classTime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassTimeExists(id))
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

        // POST: api/ClassTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassTime>> PostClassTime(ClassTime classTime)
        {
            _context.ClassTimes.Add(classTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassTime", new { id = classTime.ClassTimeId }, classTime);
        }

        // DELETE: api/ClassTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassTime(int id)
        {
            var classTime = await _context.ClassTimes.FindAsync(id);
            if (classTime == null)
            {
                return NotFound();
            }

            _context.ClassTimes.Remove(classTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassTimeExists(int id)
        {
            return _context.ClassTimes.Any(e => e.ClassTimeId == id);
        }
    }
}
