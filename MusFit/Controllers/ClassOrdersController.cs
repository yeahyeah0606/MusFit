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
    public class ClassOrdersController : ControllerBase
    {
        private readonly MusFitContext _context;

        public ClassOrdersController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/ClassOrders
        [HttpGet("guestorders/")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetGuestOrders()
        {
            var obj = from o in _context.ClassOrders
                      join s in _context.Students on o.SId equals s.SId
                      join t in _context.ClassTimes on o.ClassTimeId equals t.ClassTimeId
                      join c in _context.Classes on t.CId equals c.CId
                      join ct in _context.Terms on t.TId equals ct.TId
                      where s.SIsStudentOrNot == false
                      orderby o.OrderTime descending
                      select new
                      {
                          orderID = o.OrderId,
                          sID = s.SId,
                          name = s.SName,
                          gender = s.SGender,
                          phone = s.SPhone,
                          mail = s.SMail,
                          cID = c.CId,
                          timeID = t.ClassTimeId,
                          className = c.CName,
                          eID = o.EId,
                          date = t.CtDate,
                          weekday = t.Weekday,
                          startTime = ct.TStartTime.ToString().Substring(0, 5),
                          endTime = ct.TEndTime.ToString().Substring(0, 5),
                          orderStatus = o.OrderStatus,
                          orderTime = o.OrderTime
                      };
            return await obj.ToListAsync();
        }

        [HttpGet("guestorders/datequery/{start}/{end}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetGuestOrdersByDateQuery(DateTime start, DateTime end)
        {
            var obj = from o in _context.ClassOrders
                      join s in _context.Students on o.SId equals s.SId
                      join t in _context.ClassTimes on o.ClassTimeId equals t.ClassTimeId
                      join c in _context.Classes on t.CId equals c.CId
                      join ct in _context.Terms on t.TId equals ct.TId
                      where s.SIsStudentOrNot == false && t.CtDate >= start && t.CtDate <= end
                      orderby o.OrderTime descending
                      select new
                      {
                          orderID = o.OrderId,
                          sID = s.SId,
                          name = s.SName,
                          gender = s.SGender,
                          phone = s.SPhone,
                          mail = s.SMail,
                          cID = c.CId,
                          timeID = t.ClassTimeId,
                          className = c.CName,
                          eID = o.EId,
                          date = t.CtDate,
                          weekday = t.Weekday,
                          startTime = ct.TStartTime.ToString().Substring(0, 5),
                          endTime = ct.TEndTime.ToString().Substring(0, 5),
                          orderStatus = o.OrderStatus,
                          orderTime = o.OrderTime
                      };
            return await obj.ToListAsync();
        }

        [HttpGet("sID/{id}")]
        public async Task<ActionResult<dynamic>> GetClassOrderBysId(int id)
        {
            var obj = await _context.ClassOrders.Where(o => o.SId.Equals(id)).ToListAsync();
            return obj;
        }


        //GET: api/ClassOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetClassOrder(int id)
        {
            var obj = from o in _context.ClassOrders
                      join s in _context.Students on o.SId equals s.SId
                      join t in _context.ClassTimes on o.ClassTimeId equals t.ClassTimeId
                      join c in _context.Classes on t.CId equals c.CId
                      where o.OrderId == id
                      select new
                      {
                          orderID = o.OrderId,
                          sID = s.SId,
                          name = s.SName,
                          gender = s.SGender,
                          phone = s.SPhone,
                          mail = s.SMail,
                          cID = c.CId,
                          timeID = t.ClassTimeId,
                          className = c.CName,
                          eID = o.EId,
                          date = t.CtDate,
                          weekday = t.Weekday,
                          orderStatus = o.OrderStatus,
                          orderTime = o.OrderTime
                      };
            return await obj.ToListAsync();
        }

        // PUT: api/ClassOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassOrder(int id, ClassOrder classOrder)
        {
            if (id != classOrder.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(classOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassOrderExists(id))
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

        // POST: api/ClassOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassOrder>> PostClassOrder(ClassOrder classOrder)
        {
            _context.ClassOrders.Add(classOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassOrder", new { id = classOrder.OrderId }, classOrder);
        }

        // DELETE: api/ClassOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassOrder(int id)
        {
            var classOrder = await _context.ClassOrders.FindAsync(id);
            if (classOrder == null)
            {
                return NotFound();
            }

            _context.ClassOrders.Remove(classOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassOrderExists(int id)
        {
            return _context.ClassOrders.Any(e => e.OrderId == id);
        }
    }
}
