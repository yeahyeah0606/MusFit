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

        [HttpGet("getClassesAvailable/")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetClassesAvailable()
        {
			var result = from c in _context.Classes
						 join ct in _context.ClassTimes on c.CId equals ct.CId
						 where ct.CtLession == c.CTotalLession && ct.CtDate > DateTime.Now
						 select c;
            return await result.ToListAsync();
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

		//--------------------------------------------------------- 君 START ------------------------------------------------------------

		#region 全部可報名的課程
		// GET: api/Classes/classList
		[HttpGet("classList/")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetClassList()
		{
			var obj = from c in _context.Classes
					  join ct in _context.ClassTimes on c.CId equals ct.CId
					  join t in _context.Terms on ct.TId equals t.TId
					  join e in _context.Employees on c.EId equals e.EId
					  join r in _context.Rooms on c.RoomId equals r.RoomId
					  join lc in _context.LessionCategories on c.LcId equals lc.LcId
					  where ct.CtLession == 1 && c.CId != 11
					  orderby ct.CtDate descending
					  select new
					  {
						  cID = c.CId,
						  //cClassTimeId = ct.ClassTimeId,
						  cNumber = c.CNumber,
						  cName = c.CName,
						  ctDate = ct.CtDate,
						  tID = t.TId,
						  eEngName = e.EEngName,
						  roomName = r.RoomName,
						  cTotalLession = c.CTotalLession,
						  cExpect = c.CExpect,
						  cActual = c.CActual
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 單一課程相關資訊
		// GET: api/Classes/classList/1
		[HttpGet("classList/{id}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetClassList(int id)
		{
			var obj = from c in _context.Classes
					  join ct in _context.ClassTimes on c.CId equals ct.CId
					  join t in _context.Terms on ct.TId equals t.TId
					  join e in _context.Employees on c.EId equals e.EId
					  join r in _context.Rooms on c.RoomId equals r.RoomId
					  join lc in _context.LessionCategories on c.LcId equals lc.LcId
					  where ct.CtLession == 1 & c.CId == id
					  select new
					  {
						  cID = id,
						  cNumber = c.CNumber,
						  cName = c.CName,
						  ctDate = ct.CtDate,
						  tID = t.TId,
						  eEngName = e.EEngName,
						  eID = e.EId,
						  roomName = r.RoomName,
						  cTotalLession = c.CTotalLession,
						  cExpect = c.CExpect,
						  cActual = c.CActual,
						  lcName = lc.LcName,
						  cPrice = c.Cprice
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 綜合查詢 可報名課程
		// GET: api/Classes/ClassQuery/2023-12-01/2023-02-01/1/1
		[HttpGet("ClassQuery/{dateStart}/{dateEnd}/{lcID}/{eID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> ClassQuery(string dateStart, string dateEnd, int lcID, int eID)
		{
			var date_start = Convert.ToDateTime(dateStart);
			var date_end = Convert.ToDateTime(dateEnd);
			if (dateStart != "2999-12-31" && dateEnd != "2999-12-31" && lcID != 99 && eID != 99)
			{
				// 日期 + 課程項目 + 教練 
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && ct.CtDate >= date_start && c.CId != 11
								  && ct.CtDate <= date_end && lc.LcId == lcID && e.EId == eID
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}
			else if (dateStart != "2999-12-31" && dateEnd != "2999-12-31" && lcID != 99)
			{
				// 日期 + 課程項目
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && ct.CtDate >= date_start && c.CId != 11
								&& ct.CtDate <= date_end && lc.LcId == lcID
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}
			else if (dateStart != "2999-12-31" && dateEnd != "2999-12-31" && eID != 99)
			{
				// 日期 + 教練
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && ct.CtDate >= date_start && c.CId != 11
									&& ct.CtDate <= date_end && e.EId == eID
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}
			else if (dateStart != "2999-12-31" && dateEnd != "2999-12-31")
			{
				// 日期
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && ct.CtDate >= date_start && c.CId != 11
									&& ct.CtDate <= date_end
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}
			else if (lcID != 99 && eID != 99)
			{
				//  課程項目 + 教練
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && e.EId == eID && lc.LcId == lcID && c.CId != 11
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}
			else if (lcID != 99)
			{
				//  課程項目
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && lc.LcId == lcID && c.CId != 11
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}
			else
			{
				// 教練
				var obj = from c in _context.Classes
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join e in _context.Employees on c.EId equals e.EId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  where ct.CtLession == 1 && e.EId == eID && c.CId != 11
						  orderby c.CNumber ascending
						  select new
						  {
							  cID = c.CId,
							  cNumber = c.CNumber,
							  cName = c.CName,
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  eEngName = e.EEngName,
							  roomName = r.RoomName,
							  cTotalLession = c.CTotalLession,
							  cExpect = c.CExpect,
							  cActual = c.CActual
						  };
				return await obj.ToListAsync();
			}

		}
		#endregion

		#region 取到該課程有幾堂 cid---> classTimeId
		// GET: api/Classes/totalLession/6
		[HttpGet("totalLession/{id}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetTotalLession(int id)
		{
			var obj = from c in _context.Classes
					  join ct in _context.ClassTimes on c.CId equals ct.CId
					  join e in _context.Employees on c.EId equals e.EId
					  where c.CId == id
					  orderby ct.ClassTimeId ascending
					  select new
					  {
						  cID = id,
						  eID = e.EId,
						  cClassTimeId = ct.ClassTimeId,
						  cTotalLession = c.CTotalLession,
						  ctDate = ct.CtDate
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 取得該類型已有的課程
		// GET: api/Classes/lcCount/5
		[HttpGet("lcCount/{lcID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetlcCount(int lcID)
		{
			var obj = from c in _context.Classes
					  where c.LcId == lcID && c.CId != 11
					  orderby c.CId descending
					  select c;

			return await obj.ToListAsync();
		}
		#endregion

		#region 給cNumber取得該課程cid
		// GET: api/Classes/getid/AY003
		[HttpGet("getid/{lcNumber}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetlcID(string lcNumber)
		{
			var obj = from c in _context.Classes
					  where c.CNumber == lcNumber
					  select c;

			return await obj.ToListAsync();
		}
		#endregion


		//---------------------------------------------------------  君 END  ------------------------------------------------------------


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
