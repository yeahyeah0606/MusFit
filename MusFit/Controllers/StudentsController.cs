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
    public class StudentsController : ControllerBase
    {
        private readonly MusFitContext _context;

        public StudentsController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpGet("guests/")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllGuests()
        {
            var guests = from s in _context.Students
                         where s.SIsStudentOrNot == false
                         select s;
            return await guests.ToListAsync();
        }

        [HttpGet("guestsName/{name}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetGuestName(string name)
        {
            var guests = from s in _context.Students
                         where name == s.SName && s.SIsStudentOrNot == false
                         select s;
            return await guests.ToListAsync();
        }


        [HttpGet("findGuestsIfExist/{name}/{phone}")]
        public bool findGuestsIfExist(string name, string phone)
        {
            var result = _context.Students.FirstOrDefault(x => x.SName == name && x.SPhone == phone);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        [HttpGet("studentNumber/")]
        public IActionResult GetStudentNumber()
        {
            var students = from s in _context.Students where s.SIsStudentOrNot == true select s;
            var number = students.Count();
            return Ok(number);
        }

		[HttpGet("studentNumber/thisyear/")]
		public IActionResult GetTStudentNumberOfThisYear()
		{
			var studentsOfThisYear = from s in _context.Students where s.SIsStudentOrNot == true && s.SJoinDate.Value.Year == DateTime.Today.Year select s;
			var number = studentsOfThisYear.Count();
			return Ok(number);
		}

		[HttpGet("studentNumber/increasePercentage/")]
		public IActionResult GetInceasePercentageOfStudentNumber()
		{
			var studentsOfThisYear = from s in _context.Students where s.SIsStudentOrNot == true && s.SJoinDate.Value.Year == DateTime.Today.Year select s;
			var allStudents = from s in _context.Students where s.SIsStudentOrNot == true select s;
			var numberOfThisYear = studentsOfThisYear.Count();
			var numberOfAll = allStudents.Count();
            var difference = numberOfAll - numberOfThisYear;
            double increasePercentage = (double)numberOfThisYear / difference * 100.0;
			return Ok(increasePercentage);
		}


		[HttpGet("visitorNumber/")]
        public IActionResult GetVisitorNumber()
        {
            var visitor = from s in _context.Students where s.SIsStudentOrNot == false select s;
            var number = visitor.Count();
            return Ok(number);
        }



        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.SId)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.SId }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.SId == id);
        }
    }
}
