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
    public class CoachSpecialsController : ControllerBase
    {
        private readonly MusFitContext _context;

        public CoachSpecialsController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/CoachSpecials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoachSpecial>>> GetCoachSpecials()
        {
            return await _context.CoachSpecials.ToListAsync();
        }

        // GET: api/CoachSpecials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoachSpecial>> GetCoachSpecial(int id)
        {
            var coachSpecial = await _context.CoachSpecials.FindAsync(id);

            if (coachSpecial == null)
            {
                return NotFound();
            }

            return coachSpecial;
        }

		//--------------------------------------------------------- 君 START ------------------------------------------------------------

		#region 取到該課程類型的教練
		// GET: api/CoachSpecials/lcCoach/5
		[HttpGet("lcCoach/{lcID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetTotalLession(int lcID)
		{
			var obj = from co in _context.CoachSpecials
					  join e in _context.Employees on co.EId equals e.EId
					  join lc in _context.LessionCategories on co.LcId equals lc.LcId
					  where co.LcId == lcID
                      orderby e.EId ascending
					  select new
					  {
						  csID = co.CsId,
						  eID = co.EId,
						  lcID = co.LcId,
						  lcName = lc.LcName,
						  coach = e.EEngName
					  };

			return await obj.ToListAsync();
		}
		#endregion

		//---------------------------------------------------------  君 END  ------------------------------------------------------------


		// PUT: api/CoachSpecials/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutCoachSpecial(int id, CoachSpecial coachSpecial)
        {
            if (id != coachSpecial.CsId)
            {
                return BadRequest();
            }

            _context.Entry(coachSpecial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoachSpecialExists(id))
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

        // POST: api/CoachSpecials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CoachSpecial>> PostCoachSpecial(CoachSpecial coachSpecial)
        {
            _context.CoachSpecials.Add(coachSpecial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoachSpecial", new { id = coachSpecial.CsId }, coachSpecial);
        }

        // DELETE: api/CoachSpecials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoachSpecial(int id)
        {
            var coachSpecial = await _context.CoachSpecials.FindAsync(id);
            if (coachSpecial == null)
            {
                return NotFound();
            }

            _context.CoachSpecials.Remove(coachSpecial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoachSpecialExists(int id)
        {
            return _context.CoachSpecials.Any(e => e.CsId == id);
        }
    }
}
