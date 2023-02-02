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
    public class RoomsController : ControllerBase
    {
        private readonly MusFitContext _context;

        public RoomsController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

		//--------------------------------------------------------- 君 START ------------------------------------------------------------

		#region 送進日期 + 時段 --> 得到能使用的教室
		// GET: api/Rooms/available/2023-01-10/16
		[HttpGet("available/{mydate}/{tID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetRoomAvailable(string mydate, int tID)
		{
			var date1 = Convert.ToDateTime(mydate);
			var obj = from c in _context.Classes
					  join e in _context.Employees on c.EId equals e.EId
					  join ct in _context.ClassTimes on c.CId equals ct.CId
					  join t in _context.Terms on ct.TId equals t.TId
					  join r in _context.Rooms on c.RoomId equals r.RoomId
					  where ct.CtDate == date1 && ct.TId == tID
					  orderby ct.CtDate ascending
					  select new
					  {
						  ctDate = ct.CtDate,
						  tID = t.TId,
						  roomName = r.RoomName,
						  roomID = r.RoomId,
						  cName = c.CName,
						  eEngName = e.EEngName,
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 全部教室資訊
		// GET: api/Rooms/roomList
		[HttpGet("roomList/")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetRoomList()
		{
			var obj = from c in _context.Classes
					  join lc in _context.LessionCategories on c.LcId equals lc.LcId
					  join e in _context.Employees on c.EId equals e.EId
					  join ct in _context.ClassTimes on c.CId equals ct.CId
					  join t in _context.Terms on ct.TId equals t.TId
					  join r in _context.Rooms on c.RoomId equals r.RoomId
					  orderby ct.CtDate descending
					  select new
					  {
						  ctDate = ct.CtDate,
						  tID = t.TId,
						  roomName = r.RoomName,
						  lcType = lc.LcType,
						  cName = c.CName,
						  eEngName = e.EEngName,
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 綜合查詢 教室
		// GET: api/Rooms/RoomQuery/2023-01-01/2023-02-01/9/5
		[HttpGet("RoomQuery/{dateStart}/{dateEnd}/{tID}/{roomID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> RoomQuery(string dateStart, string dateEnd, int tID, int roomID)
		{
			var date_start = Convert.ToDateTime(dateStart);
			var date_end = Convert.ToDateTime(dateEnd);
			if (dateStart != "2999-12-31" && dateEnd != "2999-12-31" && tID != 99 && roomID != 99)
			{
				// 日期 + 時段 +教室
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where ct.CtDate >= date_start && ct.CtDate <= date_end && r.RoomId == roomID && t.TId == tID
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}
			else if (dateStart != "2999-12-31" && dateEnd != "2999-12-31" && tID != 99)
			{
				// 日期 + 時段
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where ct.CtDate >= date_start && ct.CtDate <= date_end && t.TId == tID
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}
			else if (dateStart != "2999-12-31" && dateEnd != "2999-12-31" && roomID != 99)
			{
				// 日期 + 教室
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where ct.CtDate >= date_start && ct.CtDate <= date_end && r.RoomId == roomID
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}
			else if (dateStart != "2999-12-31" && dateEnd != "2999-12-31")
			{
				// 日期
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where ct.CtDate >= date_start && ct.CtDate <= date_end
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}
			else if (tID != 99 && roomID != 99)
			{
				// 時段 + 教室
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where r.RoomId == roomID && t.TId == tID
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}
			else if (tID != 99)
			{
				// 時段 
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where t.TId == tID
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}
			else
			{
				// 教室
				var obj = from c in _context.Classes
						  join lc in _context.LessionCategories on c.LcId equals lc.LcId
						  join e in _context.Employees on c.EId equals e.EId
						  join ct in _context.ClassTimes on c.CId equals ct.CId
						  join t in _context.Terms on ct.TId equals t.TId
						  join r in _context.Rooms on c.RoomId equals r.RoomId
						  where r.RoomId == roomID
						  orderby ct.CtDate descending
						  select new
						  {
							  ctDate = ct.CtDate,
							  tID = t.TId,
							  roomName = r.RoomName,
							  lcType = lc.LcType,
							  cName = c.CName,
							  eEngName = e.EEngName,
						  };
				return await obj.ToListAsync();
			}

		}
		#endregion

		//---------------------------------------------------------  君 END  ------------------------------------------------------------


		// PUT: api/Rooms/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.RoomId)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom", new { id = room.RoomId }, room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
        }
    }
}
