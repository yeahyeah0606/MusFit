using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusFit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VwCoachScheduleController : ControllerBase
    {
        private readonly MusFitContext _context;

        public VwCoachScheduleController(MusFitContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetCoachScheduleByeId(int id)
        {
            var obj = await _context.VwCoachSchedules.Where(e => e.CoachId.Equals(id)).ToListAsync();
            return obj;
        }
    }
}
