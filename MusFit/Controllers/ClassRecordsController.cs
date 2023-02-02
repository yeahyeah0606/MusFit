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

		//--------------------------------------------------------- 君 START ------------------------------------------------------------

		#region 給cID + sID-----> crID (+日期判斷)
		// GET: api/ClassRecords/crIDfilter/12/1039
		[HttpGet("crIDfilter/{cID}/{sID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleOrderIDfilter(int cID, int sID)
		{
			var obj = from cr in _context.ClassRecords
					  join s in _context.Students on cr.SId equals s.SId
					  join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
					  join c in _context.Classes on ct.CId equals c.CId
					  where c.CId == cID & s.SId == sID && ct.CtDate > DateTime.Now
					  select new
					  {
						  crID = cr.CrId,
						  sID = s.SId,
						  classTimeID = ct.ClassTimeId,
						  cID = c.CId
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 給cID + sID-----> crID
		// GET: api/ClassRecords/getcrID/18/1039
		[HttpGet("getcrID/{cID}/{sID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleOrderID(int cID, int sID)
		{
			var obj = from cr in _context.ClassRecords
					  join s in _context.Students on cr.SId equals s.SId
					  join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
					  join c in _context.Classes on ct.CId equals c.CId
					  where c.CId == cID & s.SId == sID
					  select new
					  {
						  crID = cr.CrId,
						  sID = s.SId,
						  classTimeID = ct.ClassTimeId,
						  cID = c.CId
					  };
			return await obj.ToListAsync();
		}
		#endregion

		#region 全部課程紀錄
		//  GET:  api/ClassRecords/allRecords
		[HttpGet("allRecords/")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetClassRecord()
		{

			var query = from cr in _context.ClassRecords
						join s in _context.Students on cr.SId equals s.SId
						join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
						join c in _context.Classes on ct.CId equals c.CId
						join lc in _context.LessionCategories on c.LcId equals lc.LcId
						join t in _context.Terms on ct.TId equals t.TId
						where c.CId != 11
						orderby ct.CtDate, s.SId ascending
						//where ct.CtDate <= DateTime.Now
						select new
						{
							CrID = cr.CrId,
							SNumber = s.SNumber,
							SName = s.SName,
							CtDate = ct.CtDate,
							LcType = lc.LcType,
							CName = c.CName,
							TId = t.TId,
							CtLession = ct.CtLession,
							CrAttendance = cr.CrAttendance,
							CrContent = cr.CrContent
						};
			return await query.ToListAsync();
		}

		#endregion

		#region 綜合查詢   課程id + 姓名 + 堂數
		//  GET:  api/ClassRecords/RecordQuery/1/鄧紫棋/1
		[HttpGet("RecordQuery/{cID}/{sName}/{ctLession}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> RecordsQuery(int cID, string sName, int ctLession)
		{
			if (cID == 9999 && sName == "沒填姓名" && ctLession == 99)
			{
				var query = from cr in _context.ClassRecords
							join s in _context.Students on cr.SId equals s.SId
							join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
							join c in _context.Classes on ct.CId equals c.CId
							join lc in _context.LessionCategories on c.LcId equals lc.LcId
							join t in _context.Terms on ct.TId equals t.TId
							//from o in _context.ClassOrders
							//where cr.SId == o.SId && cr.ClassTimeId == o.ClassTimeId
							orderby ct.CtDate, s.SId ascending
							//where ct.CtDate <= DateTime.Now
							select new
							{
								CrID = cr.CrId,
								SNumber = s.SNumber,
								SName = s.SName,
								CtDate = ct.CtDate,
								LcType = lc.LcType,
								CName = c.CName,
								TId = t.TId,
								CtLession = ct.CtLession,
								CrAttendance = cr.CrAttendance,
								CrContent = cr.CrContent
							};
				return await query.ToListAsync();
			}
			else if (cID == 9999 && sName != "沒填姓名" && ctLession == 99)
			{
				//  全部課程 +姓名
				var query = from cr in _context.ClassRecords
							join s in _context.Students on cr.SId equals s.SId
							join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
							join c in _context.Classes on ct.CId equals c.CId
							join lc in _context.LessionCategories on c.LcId equals lc.LcId
							join t in _context.Terms on ct.TId equals t.TId
							where s.SName == sName && c.CId != 11
							orderby ct.CtDate, s.SId ascending
							select new
							{
								CrID = cr.CrId,
								SNumber = s.SNumber,
								SName = s.SName,
								CtDate = ct.CtDate,
								LcType = lc.LcType,
								CName = c.CName,
								TId = t.TId,
								CtLession = ct.CtLession,
								CrAttendance = cr.CrAttendance,
								CrContent = cr.CrContent,
								ClassTimeId = ct.ClassTimeId
							};
				return await query.ToListAsync();
			}
			else if (cID != 9999 && sName != "沒填姓名" && ctLession != 99)
			{
				//  課程id + 姓名 + 堂數
				var query = from cr in _context.ClassRecords
							join s in _context.Students on cr.SId equals s.SId
							join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
							join c in _context.Classes on ct.CId equals c.CId
							join lc in _context.LessionCategories on c.LcId equals lc.LcId
							join t in _context.Terms on ct.TId equals t.TId
							where c.CId == cID && s.SName == sName
							 		 && c.CId != 11 && ctLession == ct.CtLession
							orderby ct.CtDate, s.SId ascending
							select new
							{
								CrID = cr.CrId,
								SNumber = s.SNumber,
								SName = s.SName,
								CtDate = ct.CtDate,
								LcType = lc.LcType,
								CName = c.CName,
								TId = t.TId,
								CtLession = ct.CtLession,
								CrAttendance = cr.CrAttendance,
								CrContent = cr.CrContent
							};
				return await query.ToListAsync();
			}
			else if (cID != 9999 && sName != "沒填姓名" && ctLession == 99)
			{
				//  課程id + 姓名 
				var query = from cr in _context.ClassRecords
							join s in _context.Students on cr.SId equals s.SId
							join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
							join c in _context.Classes on ct.CId equals c.CId
							join lc in _context.LessionCategories on c.LcId equals lc.LcId
							join t in _context.Terms on ct.TId equals t.TId
							where c.CId == cID && s.SName == sName && c.CId != 11
							orderby ct.CtDate, s.SId ascending
							select new
							{
								CrID = cr.CrId,
								SNumber = s.SNumber,
								SName = s.SName,
								CtDate = ct.CtDate,
								LcType = lc.LcType,
								CName = c.CName,
								TId = t.TId,
								CtLession = ct.CtLession,
								CrAttendance = cr.CrAttendance,
								CrContent = cr.CrContent
							};
				return await query.ToListAsync();
			}
			else if (cID != 9999 && sName == "沒填姓名" && ctLession != 99)
			{
				// 課程id +堂數
				var query = from cr in _context.ClassRecords
							join s in _context.Students on cr.SId equals s.SId
							join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
							join c in _context.Classes on ct.CId equals c.CId
							join lc in _context.LessionCategories on c.LcId equals lc.LcId
							join t in _context.Terms on ct.TId equals t.TId
							where c.CId == cID && ctLession == ct.CtLession && c.CId != 11
							orderby ct.CtDate, s.SId ascending
							select new
							{
								CrID = cr.CrId,
								SNumber = s.SNumber,
								SName = s.SName,
								CtDate = ct.CtDate,
								LcType = lc.LcType,
								CName = c.CName,
								TId = t.TId,
								CtLession = ct.CtLession,
								CrAttendance = cr.CrAttendance,
								CrContent = cr.CrContent
							};
				return await query.ToListAsync();
			}
			else
			{
				// 課程id +全部堂數
				var query = from cr in _context.ClassRecords
							join s in _context.Students on cr.SId equals s.SId
							join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
							join c in _context.Classes on ct.CId equals c.CId
							join lc in _context.LessionCategories on c.LcId equals lc.LcId
							join t in _context.Terms on ct.TId equals t.TId
							where c.CId == cID && c.CId != 11
							orderby ct.CtDate, s.SId ascending
							select new
							{
								CrID = cr.CrId,
								SNumber = s.SNumber,
								SName = s.SName,
								CtDate = ct.CtDate,
								LcType = lc.LcType,
								CName = c.CName,
								TId = t.TId,
								CtLession = ct.CtLession,
								CrAttendance = cr.CrAttendance,
								CrContent = cr.CrContent
							};
				return await query.ToListAsync();
			}
		}
		#endregion

		#region 單筆紀錄的相關資訊
		//  GET: api/ClassRecords/crID/1
		[HttpGet("crID/{crID}")]
		public async Task<ActionResult<IEnumerable<dynamic>>> GetSingleRecord(int crID)
		{
			var query = from cr in _context.ClassRecords
						join s in _context.Students on cr.SId equals s.SId
						join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
						join c in _context.Classes on ct.CId equals c.CId
						join lc in _context.LessionCategories on c.LcId equals lc.LcId
						join t in _context.Terms on ct.TId equals t.TId
						orderby ct.CtDate ascending
						where cr.CrId == crID
						select new
						{
							CrID = cr.CrId,
							sID = s.SId,
							classTimeId = ct.ClassTimeId,
							SNumber = s.SNumber,
							SName = s.SName,
							CtDate = ct.CtDate,
							LcType = lc.LcType,
							CName = c.CName,
							TId = t.TId,
							CtLession = ct.CtLession,
							CrAttendance = cr.CrAttendance,
							CrContent = cr.CrContent
						};
			return await query.ToListAsync();
		}
		#endregion

		#region POST一次多筆
		// POST: api/ClassRecords/Many
		[HttpPost("Many")]
		public async Task<ActionResult<ClassRecord>> PostClassRecordMany([FromBody] ClassRecord[] classRecord)
		{
			foreach (ClassRecord cr in classRecord)
			{
				_context.ClassRecords.Add(cr);

			}
			await _context.SaveChangesAsync();
			return CreatedAtAction("GetClassRecord", new { id = classRecord[0].CrId }, classRecord);
		}
		#endregion

		//---------------------------------------------------------  君 END  ------------------------------------------------------------


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
