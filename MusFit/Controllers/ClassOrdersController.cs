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
			var query = from o in _context.ClassOrders
						from cr in _context.ClassRecords
						where o.SId == cr.SId && o.ClassTimeId == cr.ClassTimeId
						join s in _context.Students on o.SId equals s.SId
						join t in _context.ClassTimes on o.ClassTimeId equals t.ClassTimeId
						join c in _context.Classes on t.CId equals c.CId
						join ct in _context.Terms on t.TId equals ct.TId
						where s.SIsStudentOrNot == false
						orderby o.OrderTime descending
						select new
						{
							orderID = o.OrderId,
							sID = o.SId,
							name = s.SName,
							gender = s.SGender,
							phone = s.SPhone,
							mail = s.SMail,
							cID = c.CId,
							timeID = cr.ClassTimeId,
							className = c.CName,
							eID = o.EId,
							date = t.CtDate,
							weekday = t.Weekday,
							startTime = ct.TStartTime.ToString().Substring(0, 5),
							endTime = ct.TEndTime.ToString().Substring(0, 5),
							orderStatus = o.OrderStatus,
							orderTime = o.OrderTime,
							classRecordID = cr.CrId
						};
			return await query.ToListAsync();
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


		[HttpGet("orderNumberOfThisYear/")]
		public IActionResult GetOrderNumberOfThisYear()
        {
            var orders = from o in _context.ClassOrders where o.OrderTime.Year == DateTime.Now.Year 
                         && o.OrderStatus == "已付款" select o;
            var orderNumber = orders.Count();
            return Ok(orderNumber);
        }

		//GET: api/ClassOrders/5
		[HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetClassOrder(int id)
        {
            var obj = from o in _context.ClassOrders
                      join s in _context.Students on o.SId equals s.SId
                      join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
                      join c in _context.Classes on ct.CId equals c.CId
					  join t in _context.Terms on ct.TId equals t.TId
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
                          timeID = ct.ClassTimeId,
                          className = c.CName,
                          eID = o.EId,
                          date = ct.CtDate,
                          weekday = ct.Weekday,
						  startTime = t.TStartTime.ToString().Substring(0, 5),
                          endTime = t.TEndTime.ToString().Substring(0, 5),
                          orderStatus = o.OrderStatus,
                          orderTime = o.OrderTime
                      };
            return await obj.ToListAsync();
        }

		//--------------------------------------------------------- 君 START ------------------------------------------------------------

		#region  全部 購買課程會員清單
		// GET: api/ClassOrders/memberList
		[HttpGet("memberList/")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetMemberList()
		{
			var obj = (from o in _context.ClassOrders
					   join s in _context.Students on o.SId equals s.SId
					   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
					   join c in _context.Classes on ct.CId equals c.CId
					   join lc in _context.LessionCategories on c.LcId equals lc.LcId
					   join t in _context.Terms on ct.TId equals t.TId
					   join r in _context.Rooms on c.RoomId equals r.RoomId
					   orderby o.OrderId, ct.CtDate descending
					   where s.SIsStudentOrNot == true & ct.CtLession == 1 && c.CId != 11
					   select new
					   {
						   sNumber = s.SNumber,
						   sName = s.SName,
						   ctDate = ct.CtDate,
						   lcType = lc.LcType,
						   cName = c.CName,
						   tID = t.TId,
						   roomName = r.RoomName,
						   CTotalLession = c.CTotalLession,
						   oOrderStatus = o.OrderStatus,
						   cID = c.CId,
						   sID = s.SId,
						   orderID = o.OrderId
					   }).Distinct().OrderByDescending(x => x.orderID);
			return await obj.ToListAsync();
		}
		#endregion

		#region 檢查是否有退款     取出退款的
		// GET: api/ClassOrders/RefundList/12/6
		[HttpGet("RefundList/{cID}/{sID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetRefundList(int cID, int sID)
		{
			var obj = (from o in _context.ClassOrders
					   join s in _context.Students on o.SId equals s.SId
					   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
					   join c in _context.Classes on ct.CId equals c.CId
					   join lc in _context.LessionCategories on c.LcId equals lc.LcId
					   join t in _context.Terms on ct.TId equals t.TId
					   join r in _context.Rooms on c.RoomId equals r.RoomId
					   orderby o.OrderId, ct.CtDate descending
					   where s.SIsStudentOrNot == true & s.SId == sID & c.CId == cID & o.OrderStatus == "退款"
					   select new
					   {
						   sNumber = s.SNumber,
						   sName = s.SName,
						   ctDate = ct.CtDate,
						   lcType = lc.LcType,
						   cName = c.CName,
						   tID = t.TId,
						   roomName = r.RoomName,
						   CTotalLession = c.CTotalLession,
						   oOrderStatus = o.OrderStatus,
						   cID = c.CId,
						   sID = s.SId,
						   orderID = o.OrderId
					   }).Distinct().OrderByDescending(x => x.orderID);
			return await obj.ToListAsync();
		}
		#endregion

		#region 綜合查詢 :  特定一位 會員的購課清單 、特定一門課程的會員清單、 特定課程特定會員
		// GET: api/ClassOrders/memList/AB001/S0001
		[HttpGet("memList/{cNumber}/{sNumber}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleMemberList2(string cNumber, string sNumber)
		{
			if (cNumber == "C9999" && sNumber == "S9999")
			{
				// 全部課程 + 全部人
				var obj = (from o in _context.ClassOrders
						   join s in _context.Students on o.SId equals s.SId
						   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
						   join c in _context.Classes on ct.CId equals c.CId
						   join lc in _context.LessionCategories on c.LcId equals lc.LcId
						   join t in _context.Terms on ct.TId equals t.TId
						   join r in _context.Rooms on c.RoomId equals r.RoomId
						   orderby o.OrderId, ct.CtDate descending
						   where s.SIsStudentOrNot == true && ct.CtLession == 1 && c.CId !=11
						   select new
						   {
							   sNumber = s.SNumber,
							   sName = s.SName,
							   ctDate = ct.CtDate,
							   lcType = lc.LcType,
							   cName = c.CName,
							   tID = t.TId,
							   roomName = r.RoomName,
							   CTotalLession = c.CTotalLession,
							   oOrderStatus = o.OrderStatus,
							   cID = c.CId,
							   sID = s.SId,
							   orderID = o.OrderId
						   }).Distinct().OrderByDescending(x => x.orderID);
				return await obj.ToListAsync();
			}
			else if (cNumber == "C9999" && sNumber != "S9999")
			{
				// 全部課程 +特定會員
				var obj = (from o in _context.ClassOrders
						   join s in _context.Students on o.SId equals s.SId
						   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
						   join c in _context.Classes on ct.CId equals c.CId
						   join lc in _context.LessionCategories on c.LcId equals lc.LcId
						   join t in _context.Terms on ct.TId equals t.TId
						   join r in _context.Rooms on c.RoomId equals r.RoomId
						   orderby o.OrderId, ct.CtDate descending
						   where s.SIsStudentOrNot == true & ct.CtLession == 1
									 && c.CId != 11 && s.SNumber == sNumber
						   select new
						   {
							   sNumber = s.SNumber,
							   sName = s.SName,
							   ctDate = ct.CtDate,
							   lcType = lc.LcType,
							   cName = c.CName,
							   tID = t.TId,
							   roomName = r.RoomName,
							   CTotalLession = c.CTotalLession,
							   oOrderStatus = o.OrderStatus,
							   oorderID = o.OrderId,
							   cID = c.CId,
							   sID = s.SId
						   }).Distinct().OrderByDescending(x => x.oorderID);
				return await obj.ToListAsync();
			}
			else if (cNumber != "C9999" && sNumber == "S9999")
			{
				// 特定課程 + 全部人
				var obj = (from o in _context.ClassOrders
						   join s in _context.Students on o.SId equals s.SId
						   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
						   join c in _context.Classes on ct.CId equals c.CId
						   join lc in _context.LessionCategories on c.LcId equals lc.LcId
						   join t in _context.Terms on ct.TId equals t.TId
						   join r in _context.Rooms on c.RoomId equals r.RoomId
						   orderby o.OrderId, ct.CtDate descending
						   where s.SIsStudentOrNot == true & ct.CtLession == 1
									 && c.CId != 11 && c.CNumber == cNumber
						   select new
						   {
							   sNumber = s.SNumber,
							   sName = s.SName,
							   ctDate = ct.CtDate,
							   lcType = lc.LcType,
							   cName = c.CName,
							   tID = t.TId,
							   roomName = r.RoomName,
							   CTotalLession = c.CTotalLession,
							   oOrderStatus = o.OrderStatus,
							   oorderID = o.OrderId,
							   cID = c.CId,
							   sID = s.SId
						   }).Distinct().OrderByDescending(x => x.oorderID);
				return await obj.ToListAsync();
			}
			else
			{
				// 特定課程  特定人
				var obj = (from o in _context.ClassOrders
						   join s in _context.Students on o.SId equals s.SId
						   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
						   join c in _context.Classes on ct.CId equals c.CId
						   join lc in _context.LessionCategories on c.LcId equals lc.LcId
						   join t in _context.Terms on ct.TId equals t.TId
						   join r in _context.Rooms on c.RoomId equals r.RoomId
						   orderby o.OrderId, ct.CtDate descending
						   where s.SIsStudentOrNot == true & ct.CtLession == 1
									&& c.CNumber == cNumber && s.SNumber == sNumber
									&& c.CId != 11
						   select new
						   {
							   sNumber = s.SNumber,
							   sName = s.SName,
							   ctDate = ct.CtDate,
							   lcType = lc.LcType,
							   cName = c.CName,
							   tID = t.TId,
							   roomName = r.RoomName,
							   CTotalLession = c.CTotalLession,
							   oOrderStatus = o.OrderStatus,
							   oorderID = o.OrderId,
							   cID = c.CId,
							   sID = s.SId
						   }).Distinct().OrderByDescending(x => x.oorderID);
				return await obj.ToListAsync();
			}
		}
		#endregion

		#region 單筆(只有第一堂課)
		// GET: api/ClassOrders/order/1/5
		[HttpGet("order/{cID}/{sID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleMemberList(int cID, int sID)
		{
			var obj = (from o in _context.ClassOrders
					   join s in _context.Students on o.SId equals s.SId
					   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
					   join c in _context.Classes on ct.CId equals c.CId
					   join lc in _context.LessionCategories on c.LcId equals lc.LcId
					   join t in _context.Terms on ct.TId equals t.TId
					   join r in _context.Rooms on c.RoomId equals r.RoomId
					   where s.SIsStudentOrNot == true & ct.CtLession == 1 && sID == s.SId && cID == c.CId
					   select new
					   {
						   sNumber = s.SNumber,
						   sName = s.SName,
						   ctDate = ct.CtDate,
						   lcType = lc.LcType,
						   cName = c.CName,
						   tID = t.TId,
						   roomName = r.RoomName,
						   CTotalLession = c.CTotalLession,
						   coOrderStatus = o.OrderStatus,
						   OorderID = o.OrderId,
						   eID = c.EId,
						   cID = c.CId
					   });
			return await obj.ToListAsync();
		}
		#endregion

		#region  單筆課程單個會員(全部堂課)
		// GET: api/ClassOrders/orderIDall/1/5
		[HttpGet("orderIDall/{cID}/{sID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleMemberList2(int cID, int sID)
		{
			var obj = (from o in _context.ClassOrders
					   join s in _context.Students on o.SId equals s.SId
					   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
					   join c in _context.Classes on ct.CId equals c.CId
					   join lc in _context.LessionCategories on c.LcId equals lc.LcId
					   join t in _context.Terms on ct.TId equals t.TId
					   join r in _context.Rooms on c.RoomId equals r.RoomId
					   where s.SIsStudentOrNot == true && sID == s.SId && cID == c.CId
					   select new
					   {
						   sNumber = s.SNumber,
						   sName = s.SName,
						   ctDate = ct.CtDate,
						   lcType = lc.LcType,
						   cName = c.CName,
						   tID = t.TId,
						   roomName = r.RoomName,
						   CTotalLession = c.CTotalLession,
						   coOrderStatus = o.OrderStatus,
						   OorderID = o.OrderId,
						   eID = c.EId,
						   cID = c.CId,
						   classTimeID = ct.ClassTimeId
					   });
			return await obj.ToListAsync();
		}
		#endregion

		#region 單一課程所有報名會員
		// GET: api/ClassOrders/singleclass/1
		[HttpGet("singleclass/{cid}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleClassMember(int cid)
		{
			var obj = (from o in _context.ClassOrders
					   join s in _context.Students on o.SId equals s.SId
					   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
					   join c in _context.Classes on ct.CId equals c.CId
					   join lc in _context.LessionCategories on c.LcId equals lc.LcId
					   join t in _context.Terms on ct.TId equals t.TId
					   join r in _context.Rooms on c.RoomId equals r.RoomId
					   orderby o.OrderId descending
					   where s.SIsStudentOrNot == true & ct.CtLession == 1 & c.CId == cid
					   select new
					   {
						   sNumber = s.SNumber,
						   sName = s.SName,
						   ctDate = ct.CtDate,
						   lcType = lc.LcType,
						   cName = c.CName,
						   tID = t.TId,
						   roomName = r.RoomName,
						   CTotalLession = c.CTotalLession,
						   oOrderStatus = o.OrderStatus,
						   cID = c.CId,
						   sID = s.SId,
						   orderID = o.OrderId
					   }).Distinct().OrderByDescending(x => x.orderID);
			return await obj.ToListAsync();
		}
		#endregion

		#region  單筆(+日期判斷)
		// GET: api/ClassOrders/filter/1/5
		[HttpGet("filter/{cID}/{sID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleMemberList3(int cID, int sID)
		{
			var obj = (from o in _context.ClassOrders
					   join s in _context.Students on o.SId equals s.SId
					   join ct in _context.ClassTimes on o.ClassTimeId equals ct.ClassTimeId
					   join c in _context.Classes on ct.CId equals c.CId
					   join lc in _context.LessionCategories on c.LcId equals lc.LcId
					   join t in _context.Terms on ct.TId equals t.TId
					   join r in _context.Rooms on c.RoomId equals r.RoomId
					   where s.SIsStudentOrNot == true && sID == s.SId && cID == c.CId && ct.CtDate > DateTime.Now
					   select new
					   {
						   sNumber = s.SNumber,
						   sName = s.SName,
						   ctDate = ct.CtDate,
						   lcType = lc.LcType,
						   cName = c.CName,
						   tID = t.TId,
						   roomName = r.RoomName,
						   CTotalLession = c.CTotalLession,
						   coOrderStatus = o.OrderStatus,
						   OorderID = o.OrderId,
						   eID = c.EId,
						   cID = c.CId,
						   classTimeID = ct.ClassTimeId
					   });
			return await obj.ToListAsync();
		}
		#endregion

		//---------------------------------------------------------  君 END  ------------------------------------------------------------


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
