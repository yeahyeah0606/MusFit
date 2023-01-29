using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusFit.EncryptPassword;
using MusFit.Models;
using MusFit.Utilities;
using MusFit.ViewModels;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

namespace MusFit.Controllers
{
    public class ManageController : Controller
    {
        private MusFitContext _context;

        public ManageController(MusFitContext context)
        {
            _context = context;
        }

        [Authentication]
        public IActionResult Index(string name, int eId)
        {
            TempData["name"] = name;
            TempData["eId"] = eId;
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Manager") == null && HttpContext.Session.GetString("Coach") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string encryptedPassword)
        {
            Decryption decryption = new Decryption();
            var password = decryption.DecryptStringAES(encryptedPassword);
            byte[] password_SHA256 = StringToSHA256(password);

            if (HttpContext.Session.GetString("Manager") == null && HttpContext.Session.GetString("Coach") == null)
            {
                if (ModelState.IsValid)
                {
                    var manager = _context.Employees.Where(e => e.EAccount.Equals(username) && e.EPassword.Equals(password_SHA256) && e.EIsCoach == false).FirstOrDefault();
                    var coach = _context.Employees.Where(e => e.EAccount.Equals(username) && e.EPassword.Equals(password_SHA256) && e.EIsCoach == true).FirstOrDefault();
                    if (manager != null)
                    {
                        HttpContext.Session.SetString("Manager", manager.EAccount.ToString());
                        return RedirectToAction("Index", new { name = manager.EName, eId = manager.EId });
                    }
                    else if (coach != null)
                    {
                        HttpContext.Session.SetString("Coach", coach.EAccount.ToString());
                        return RedirectToAction("Index", new { name = coach.EName, eId = coach.EId });
                    }
                    else
                    {
                        ViewBag.Message = "帳號或密碼輸入錯誤!!";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        private byte[] StringToSHA256(string str)
        {
            StringBuilder builder = new StringBuilder();
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(str);
            byte[] crypto = sha256.ComputeHash(source);
            return crypto;
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login");
        }

        [Authentication]
        public IActionResult CoachScheduleQuery()
        {
            return View();
        }
        [Authentication]
        public IActionResult CoachSchedule(int id)
        {
            ViewBag.eId = id;
            return View();
        }

        [Authentication]
        public IActionResult VisitorInfo()
        {
            return View();
        }

        [Authentication]
        public IActionResult InBodyRecords(long? id = 1)
        {
            ViewBag.Id = id;
            return View();
        }
        [Authentication]
        public IActionResult InBodyPersonalRecords(long? id)
        {
            ViewBag.Id = id;
            return View();
        }

        [Authentication]
        public IActionResult InBodyRecordQuery()
        {
            return View();
        }


        public IActionResult News()
        {
            var myquery2 = (from myemployee1 in _context.Employees select myemployee1).ToList();
            var myquery = (from Mynews in _context.News select Mynews.NId).ToList();
            ViewBag.querydata = myquery2;
            ViewBag.Myquerydata = myquery;





            var viewModel = _context.News.ToList();

            return View(viewModel);
        }


      
        public IActionResult StudentQuery()
        {

            return View();

        }

       
        public IActionResult StudentSearch(string SNumber, string SName, string SAccount)
        {
            var queryResult = (from s in _context.Students
                               where s.SIsStudentOrNot == true && (s.SNumber == SNumber || s.SName == SName || s.SAccount == SAccount)
                               select new StudentViewModel()
                               {
                                   SId = s.SId,
                                   SNumber = s.SNumber,
                                   SName = s.SName,
                                   SMail = s.SMail,
                                   SBirth = s.SBirth,
                                   SGender = s.SGender,
                                   SContactor = s.SContactor,
                                   SContactPhone = s.SContactPhone,
                                   SPhoto = s.SPhoto,
                                   SAddress = s.SAddress,
                                   SPhone = s.SPhone,
                                   SAccount = s.SAccount,
                                   SToken = s.SToken,
                                   SJoinDate = s.SJoinDate,
                                   SIsStudentOrNot = s.SIsStudentOrNot
                               }).FirstOrDefault();

            if (queryResult == null)
            {
                ViewData["error"] = "*請輸入正確會員資料*";
                return View("StudentQuery");
            }

            return View("StudentSelect", queryResult);
        }

        [HttpPost]
        public IActionResult GetStudentData(Student student)
        {
            try
            {
                var json = "";
                var studentResult =  _context.Students.FirstOrDefault(u => u.SPhone == student.SPhone);

                if (studentResult != null)
                {
                    //將物件轉成json 格式的字串
                    json = JsonConvert.SerializeObject(studentResult);

                    //將json字串轉成物件 => 可使用物件裡面的屬性
                    //Student aa = JsonConvert.DeserializeObject<Student>(json);
                }

                return Json(json);
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

       
        public IActionResult StudentSelect(string SNumber, string SName, string SAccount)
        {
            try
            {
                var queryResult = (from s in _context.Students
                                   where s.SIsStudentOrNot == true && (s.SNumber == SNumber || s.SName == SName || s.SAccount == SAccount)
                                   select new StudentViewModel()
                                   {
                                       SId = s.SId,
                                       SNumber = s.SNumber,
                                       SName = s.SName,
                                       SMail = s.SMail,
                                       SBirth = s.SBirth,
                                       SGender = s.SGender,
                                       SContactor = s.SContactor,
                                       SContactPhone = s.SContactPhone,
                                       SPhoto = s.SPhoto,
                                       SAddress = s.SAddress,
                                       SPhone = s.SPhone,
                                       SAccount = s.SAccount,
                                       SToken = s.SToken,
                                       SJoinDate = s.SJoinDate,
                                       SIsStudentOrNot = s.SIsStudentOrNot
                                   }).FirstOrDefault();

               
                return View("StudentSelect", queryResult);
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

   
        public IActionResult StudentEdit()
        {
            return View();
        }

     
        public IActionResult SEdit(string SNumber)
        {
            try
            {
                
                var queryResult = (from s in _context.Students
                                   where s.SNumber == SNumber
                                   select new StudentViewModel()
                                   {
                                       SId = s.SId,
                                       SNumber = s.SNumber,
                                       SName = s.SName,
                                       SMail = s.SMail,
                                       SBirth = s.SBirth,
                                       SGender = s.SGender,
                                       SContactor = s.SContactor,
                                       SContactPhone = s.SContactPhone,
                                       SPhoto = s.SPhoto,
                                       SAddress = s.SAddress,
                                       SPhone = s.SPhone,
                                       SAccount = s.SAccount,
                                       SToken = s.SToken,
                                       SJoinDate = s.SJoinDate,
                                       SIsStudentOrNot = s.SIsStudentOrNot
                                   }).FirstOrDefault();

                return View("StudentEdit", queryResult);
            }
            catch (System.Exception e)
            {

                throw e;
            }

        }

        [HttpPost]
        public async Task<IActionResult> SEdit(StudentViewModel student, [FromForm(Name = "SPhoto")] IFormFile SPhoto)
        {
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    if (SPhoto != null && SPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            SPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            student.SPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }
                    else
                    {
                        var user = await _context.Students.FirstOrDefaultAsync(u => u.SAccount == student.SAccount);
                        student.SPhoto = user.SPhoto;
                    }
                    return View("StudentEdit", student);
                }
                else
                {

                    var studentResult = await _context.Students.FirstOrDefaultAsync(u => u.SNumber == student.SNumber);

                    studentResult.SName = student.SName;
                    studentResult.SMail = student.SMail;
                    studentResult.SBirth = student.SBirth;
                    studentResult.SGender = (bool)student.SGender;
                    studentResult.SContactor = student.SContactor;
                    studentResult.SContactPhone = student.SContactPhone;
                    studentResult.SAddress = student.SAddress;
                    studentResult.SPhone = student.SPhone;
                    studentResult.SAccount = student.SAccount;
                    studentResult.SJoinDate = student.SJoinDate;
                    student.SPhoto = studentResult.SPhoto;

                    if (!string.IsNullOrEmpty(student.SPassword))
                    {
                        // 轉換 password -> sha2_256 比較
                        byte[] data = Encoding.GetEncoding(1252).GetBytes(student.SPassword);
                        var sha = new SHA256Managed();
                        byte[] bytesEncode = sha.ComputeHash(data);
                        studentResult.SPassword = bytesEncode;
                    }

                    if (SPhoto != null && SPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await SPhoto.CopyToAsync(ms);
                            var fileBytes = ms.ToArray();
                            studentResult.SPhoto = Convert.ToBase64String(fileBytes);
                            student.SPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }


                    if (studentResult != null)
                    {
                        await _context.SaveChangesAsync();
                    }

                    return View("StudentSelect", student);
                }
            }
        }


     
        public IActionResult StudentAdd()
        {
            return View();
        }

        public async Task<IActionResult> SAdd(string SPhone)
        {
            var guestResult = await(from s in _context.Students
                               where s.SPhone == SPhone && s.SIsStudentOrNot == false
                               select new StudentViewModel()
                               {
                                   SNumber = s.SNumber,
                                   SName = s.SName,
                                   SMail = s.SMail,
                                   SGender = s.SGender,
                                   SBirth = s.SBirth,
                                   SPhone = s.SPhone,
                                   SPhoto = s.SPhoto,
                                   SAccount = s.SAccount,
                                   SContactor = s.SContactor,
                                   SContactPhone = s.SContactPhone,
                                   SAddress = s.SAddress,
                                   SJoinDate = s.SJoinDate,
                                   SIsStudentOrNot = s.SIsStudentOrNot
                               }).FirstOrDefaultAsync();


            return View("StudentAdd", guestResult);
        }


        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel, [FromForm(Name = "SPhoto")] IFormFile SPhoto)
        {
            if (ModelState.IsValid)
            {
                //是否為訪客資料
                var guestResult = await _context.Students.FirstOrDefaultAsync(x => x.SPhone == studentViewModel.SPhone);
            

                //轉換 password -> sha2_256 比較
                string sPassword = studentViewModel.SBirth.ToString().Replace("-","").Replace("-","");
                byte[] data = Encoding.GetEncoding(1252).GetBytes(sPassword);
                var sha = new SHA256Managed();
                byte[] bytesEncode = sha.ComputeHash(data);

               
                
                Student student = new Student()
                {
                    SNumber = studentViewModel.SNumber,
                    SName = studentViewModel.SName,
                    SMail = studentViewModel.SMail,
                    SGender = studentViewModel.SGender,
                    SBirth = studentViewModel.SBirth,
                    SPhone = studentViewModel.SPhone,
                    SPhoto = studentViewModel.SPhoto,
                    SAccount = studentViewModel.SAccount,
                    SContactor = studentViewModel.SContactor,
                    SContactPhone = studentViewModel.SContactPhone,
                    SAddress = studentViewModel.SAddress,
                    SJoinDate = studentViewModel.SJoinDate,
                    SIsStudentOrNot = true,
                    SPassword = bytesEncode
                };

                studentViewModel.SIsStudentOrNot = true;
                //不是訪客
                if (guestResult == null)
                {
                    if (SPhoto != null && SPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            SPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            studentViewModel.SPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }
                    await _context.Students.AddAsync(student);
                    await _context.SaveChangesAsync();
                    studentViewModel.SNumber =  _context.Students.FirstOrDefault(x => x.SPhone == studentViewModel.SPhone).SNumber;
                    return View("StudentSelect", studentViewModel);

                }
                else
                {
                    if (SPhoto != null && SPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            SPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            studentViewModel.SPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }


                    studentViewModel.SNumber = guestResult.SNumber;
                    guestResult.SName = studentViewModel.SName;
                    guestResult.SMail = studentViewModel.SMail;
                    guestResult.SBirth = studentViewModel.SBirth;
                    guestResult.SGender = (bool)studentViewModel.SGender;
                    guestResult.SContactor = studentViewModel.SContactor;
                    guestResult.SContactPhone = studentViewModel.SContactPhone;
                    guestResult.SAddress = studentViewModel.SAddress;
                    guestResult.SPhone = studentViewModel.SPhone;
                    guestResult.SAccount = studentViewModel.SAccount;
                    guestResult.SJoinDate = studentViewModel.SJoinDate;
                    guestResult.SPhoto = studentViewModel.SPhoto;
                    guestResult.SIsStudentOrNot = studentViewModel.SIsStudentOrNot;


                    await _context.SaveChangesAsync();
                    return View("StudentSelect", studentViewModel);

                }


            }
            else
            {
                return View("StudentAdd");
            }
        }

       
        public IActionResult EmployeeQuery()
        {
            return View();
        }

        
        public IActionResult EmployeeSearch(string ENumber, string EName, string EAccount)
        {
            var queryResult = (from e in _context.Employees
                               where (e.ENumber == ENumber || e.EName == EName || e.EAccount == EAccount ) && e.EResignDate < DateTime.Now
                               select new EmployeeViewModel()
                               {
                                   ENumber = e.ENumber,
                                   EName = e.EName,
                                   EEngName = e.EEngName,
                                   EMail = e.EMail,
                                   EGender = e.EGender,
                                   EBirth = e.EBirth,
                                   EIdentityNumber = e.EIdentityNumber,
                                   EPhone = e.EPhone,
                                   EPhoto = e.EPhoto,
                                   EAccount = e.EAccount,
                                   EContactor = e.EContactor,
                                   EContactorPhone = e.EContactorPhone,
                                   EAddress = e.EAddress,
                                   EEnrollDate = e.EEnrollDate,
                                   EResignDate = e.EResignDate,
                                   EIsCoach = e.EIsCoach,
                                   EExplain = e.EExplain
                               }).FirstOrDefault();

            if (queryResult == null)
            {
                ViewData["error"] = "*請輸入正確會員資料*";
                return View("EmployeeQuery");
            }

            //全部課程
            queryResult.AvailableLession = GetLession();

            var employeeLessionResult = (from e in _context.Employees
                                         join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                         from cs in esc.DefaultIfEmpty()
                                         where e.ENumber == ENumber
                                         select cs.LcId.ToString()
                               ).ToList();
            //已選課程
            queryResult.SelectedLession = employeeLessionResult;

            return View("EmployeeSelect", queryResult);
        }

        
        public IActionResult EmployeeSelect()
        {
            return View();
        }

       
        public IActionResult EmployeeEdit()
        {

            return View();
        }

       

        public IActionResult EEdit(string ENumber)
        {

            var queryResult = (from e in _context.Employees
                               where e.ENumber == ENumber
                               select new EmployeeViewModel()
                               {
                                   ENumber = e.ENumber,
                                   EName = e.EName,
                                   EEngName = e.EEngName,
                                   EMail = e.EMail,
                                   EGender = e.EGender,
                                   EBirth = e.EBirth,
                                   EIdentityNumber = e.EIdentityNumber,
                                   EPhone = e.EPhone,
                                   EPhoto = e.EPhoto,
                                   EAccount = e.EAccount,
                                   EContactor = e.EContactor,
                                   EContactorPhone = e.EContactorPhone,
                                   EAddress = e.EAddress,
                                   EEnrollDate = e.EEnrollDate,
                                   EResignDate = e.EResignDate,
                                   EIsCoach = e.EIsCoach,
                                   EExplain = e.EExplain
                               }).FirstOrDefault();



            var employeeLessionResult = (from e in _context.Employees
                                         join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                         from cs in esc.DefaultIfEmpty()
                                         where e.ENumber == ENumber
                                         select cs.LcId.ToString()
                               ).ToList();

            queryResult.AvailableLession = GetLession();
            queryResult.SelectedLession = employeeLessionResult;

            return View("EmployeeEdit", queryResult);

        }

        [HttpPost]
        public async Task<IActionResult> EEdit(EmployeeViewModel employee, [FromForm(Name = "EPhoto")] IFormFile EPhoto)
        {
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var LessionResult = (from e in _context.Employees
                                        join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                        from cs in esc.DefaultIfEmpty()
                                        where e.ENumber == employee.ENumber
                                         select cs.LcId.ToString()
                               ).ToList();

                    employee.AvailableLession = GetLession();
                    employee.SelectedLession = LessionResult;

                    if (EPhoto != null && EPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            EPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            employee.EPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }
                    else
                    {
                        var user = await _context.Employees.FirstOrDefaultAsync(u => u.EAccount == employee.EAccount);
                        employee.EPhoto = user.EPhoto;
                    }
                    return View("EmployeeEdit", employee);
                }
                else
                {

                    var employeeResult = await _context.Employees.FirstOrDefaultAsync(u => u.ENumber == employee.ENumber);

                    employeeResult.EName = employee.EName;
                    employeeResult.EEngName = employee.EEngName;
                    employeeResult.EGender = (bool)employee.EGender;
                    employeeResult.EBirth = (DateTime)employee.EBirth;
                    employeeResult.EIdentityNumber = employee.EIdentityNumber;
                    employeeResult.EPhone = employee.EPhone;
                    employeeResult.EMail = employee.EMail;
                    employeeResult.EAccount = employee.EAccount;
                    employeeResult.EContactor = employee.EContactor;
                    employeeResult.EContactorPhone = employee.EContactorPhone;
                    employeeResult.EAddress = employee.EAddress;
                    employeeResult.EEnrollDate = employee.EEnrollDate;
                    employeeResult.EResignDate = employee.EResignDate;
                    employeeResult.EIsCoach = (bool)employee.EIsCoach;
                    employeeResult.EExplain = employee.EExplain;
                    employee.EPhoto = employeeResult.EPhoto;

                    //若密碼重設欄有字串表示要重設密碼 
                    if (!string.IsNullOrEmpty(employee.EPassword))
                    {
                        // 轉換 password -> sha2_256 比較
                        byte[] data = Encoding.GetEncoding(1252).GetBytes(employee.EPassword);
                        var sha = new SHA256Managed();
                        byte[] bytesEncode = sha.ComputeHash(data);
                        employeeResult.EPassword = bytesEncode;
                    }

                    if (EPhoto != null && EPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            EPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            employeeResult.EPhoto = Convert.ToBase64String(fileBytes);
                            employee.EPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }
                   

                    if (employeeResult != null)
                    {
                        await _context.SaveChangesAsync();
                    }



                    var oldCoachSpecials = (from e in _context.Employees
                                            join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                            from cs in esc.DefaultIfEmpty()
                                            where e.ENumber == employee.ENumber
                                            select cs
                                   ).ToList();

                    //移除一個List
                    _context.CoachSpecials.RemoveRange(oldCoachSpecials);
                    _context.SaveChanges();


                    var NewCoachSpecials = new List<CoachSpecial>();

                    //增加被勾選的課程
                    foreach (var item in employee.SelectedLession)
                    {
                        NewCoachSpecials.Add(new CoachSpecial() { EId = employeeResult.EId, LcId = int.Parse(item) });
                    }

                    _context.CoachSpecials.AddRange(NewCoachSpecials);
                    _context.SaveChanges();

                    employee.AvailableLession = GetLession();
                    return View("EmployeeSelect", employee);
                }
            }


        }

      
        //取得6筆課程項目
        private IList<SelectListItem> GetLession()
        {
            var queryList = _context.LessionCategories.AsNoTracking().ToList();
            List<SelectListItem> lessionCategories = new List<SelectListItem>();

            foreach (var item in queryList)
            {
                lessionCategories.Add(new SelectListItem()
                {
                    Text = item.LcName,
                    Value = item.LcId.ToString()
                });
            }
            return lessionCategories;
        }

      
        public IActionResult EmployeeAdd()
        {
            var queryResult = new EmployeeViewModel();
            queryResult.AvailableLession = GetLession();

            return View("EmployeeAdd", queryResult);
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeData(Employee employee)
        {
            var json = "";
            var employeeResult = await (from e in _context.Employees
                                        join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                        from cs in esc.DefaultIfEmpty()
                                        where e.EIdentityNumber == employee.EIdentityNumber
                                        select cs).FirstOrDefaultAsync();

            if (employeeResult != null)
            {
                //將物件轉成json 格式的字串
                json = JsonConvert.SerializeObject(employeeResult);

                //將json字串轉成物件 => 可使用物件裡面的屬性
                //Student aa = JsonConvert.DeserializeObject<Student>(json);
            }

            return Json(json);
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeAdd(EmployeeViewModel employeeViewModel, [FromForm(Name = "EPhoto")] IFormFile EPhoto)
        {
            var isExistEmployee = await _context.Employees.FirstOrDefaultAsync(u => u.EIdentityNumber == employeeViewModel.EIdentityNumber);
            //若員工已存在
            if (isExistEmployee != null)
            {
                var queryResult = new EmployeeViewModel();
                queryResult.AvailableLession = GetLession();

                return View("EmployeeAdd", queryResult);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                   
                    return View("EmployeeAdd");
                }
                else
                {

                    // 轉換 password -> sha2_256 比較
                    string ePassword = employeeViewModel.EBirth.ToString().Replace("-", "").Replace("-", "");
                    byte[] data = Encoding.GetEncoding(1252).GetBytes(ePassword);
                    var sha = new SHA256Managed();
                    byte[] bytesEncode = sha.ComputeHash(data);

                    

                    Employee emp = new Employee()
                    {
                        ENumber = employeeViewModel.ENumber,
                        EName = employeeViewModel.EName,
                        EEngName = employeeViewModel.EEngName,
                        EGender = (bool)employeeViewModel.EGender,
                        EBirth = (DateTime)employeeViewModel.EBirth,
                        EIdentityNumber = employeeViewModel.EIdentityNumber,
                        EPhone = employeeViewModel.EPhone,
                        EMail = employeeViewModel.EMail,
                        EAccount = employeeViewModel.EAccount,
                        EPassword = bytesEncode,
                        EContactor = employeeViewModel.EContactor,
                        EContactorPhone = employeeViewModel.EContactorPhone,
                        EAddress = employeeViewModel.EAddress,
                        EEnrollDate = employeeViewModel.EEnrollDate,
                        EResignDate = employeeViewModel.EResignDate,
                        EIsCoach = (bool)employeeViewModel.EIsCoach,
                        EExplain = employeeViewModel.EExplain,
                        EPhoto = employeeViewModel.EPhoto,
                    };

                    _context.Employees.Add(emp);
                    _context.SaveChanges();

                    if (EPhoto != null && EPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            EPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            employeeViewModel.EPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }

                    //搜尋剛剛新增員工的id
                    var employeeResult = await _context.Employees.FirstOrDefaultAsync(u => u.EIdentityNumber == employeeViewModel.EIdentityNumber);
                    var NewCoachSpecials = new List<CoachSpecial>();

                    foreach (var item in employeeViewModel.SelectedLession)
                    {
                        NewCoachSpecials.Add(new CoachSpecial() { EId = employeeResult.EId, LcId = int.Parse(item) });
                    }

                    employeeViewModel.ENumber = _context.Employees.FirstOrDefault(u => u.EIdentityNumber == employeeViewModel.EIdentityNumber).ENumber;
                    _context.CoachSpecials.AddRange(NewCoachSpecials);
                    _context.SaveChanges();
                }
            }

            employeeViewModel.AvailableLession = GetLession();
            return View("EmployeeSelect", employeeViewModel);
        }

        [Authentication]
        public IActionResult EditPassword()
        {
            string eAccount = HttpContext.Session.GetString("Manager") ?? HttpContext.Session.GetString("Coach");
            if (eAccount == "Coach")
            {
                return Redirect("/Manage/Index");
            }
            return View();
        }


        [HttpPost]
        [Authentication]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel password)
        {
            if (ModelState.IsValid)
            {
                
                // 轉換 password -> sha2_256 比較 (轉使用者輸入的舊密碼與資料庫比較)
                byte[] data = Encoding.GetEncoding(1252).GetBytes(password.OldPassword);
                var sha = new SHA256Managed();
                byte[] bytesEncode = sha.ComputeHash(data);

                string eAccount = HttpContext.Session.GetString("Manager") ?? HttpContext.Session.GetString("Coach");

                var query = await _context.Employees.FirstOrDefaultAsync(
                                   x => x.EPassword == bytesEncode &&
                                        x.EAccount == eAccount);


                bool isDataError = false;
                if (query == null)
                {
                    ViewData["errorOld"] = "舊密碼輸入錯誤!";
                    isDataError = true;
                }
                
                if (password.NewPassword != password.CheckPassword)
                {
                    ViewData["errorNew"] = "新密碼與確認密碼不符!";
                    isDataError = true;
                }
                
                if (password.OldPassword == password.NewPassword)
                {
                    ViewData["errorDouble"] = "舊密碼與新密碼不可以一樣!";
                    isDataError = true;
                }

                if (isDataError)
                {
                    return View("EditPassword");

                }

                // 轉換 password -> sha2_256 比較  (轉新密碼存進資料庫)
                data = Encoding.GetEncoding(1252).GetBytes(password.NewPassword);
                bytesEncode = sha.ComputeHash(data);
                query.EPassword = bytesEncode;
                await _context.SaveChangesAsync();

                ViewData["message"] = "密碼修改成功!";
                return View("EditPassword");
            }else
            {
                
                return View("EditPassword");
            }
        }

        [Authentication]
        public IActionResult CoachEdit()
        {
            string eAccount = HttpContext.Session.GetString("Manager")?? HttpContext.Session.GetString("Coach");
            if (eAccount == "Manager")
            {
                return Redirect("/Manage/Index");
            }
            var queryResult = (from e in _context.Employees
                               where e.EAccount == eAccount
                               select new EmployeeViewModel()
                               {
                                   ENumber = e.ENumber,
                                   EName = e.EName,
                                   EEngName = e.EEngName,
                                   EMail = e.EMail,
                                   EGender = e.EGender,
                                   EBirth = e.EBirth,
                                   EIdentityNumber = e.EIdentityNumber,
                                   EPhone = e.EPhone,
                                   EPhoto = e.EPhoto,
                                   EAccount = e.EAccount,
                                   EContactor = e.EContactor,
                                   EContactorPhone = e.EContactorPhone,
                                   EAddress = e.EAddress,
                                   EEnrollDate = e.EEnrollDate,
                                   EResignDate = e.EResignDate,
                                   EIsCoach = e.EIsCoach,
                                   EExplain = e.EExplain
                               }).FirstOrDefault();



            var CoachLessionResult = (from e in _context.Employees
                                         join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                         from cs in esc.DefaultIfEmpty()
                                         where e.EAccount == eAccount
                                         select cs.LcId.ToString()
                               ).ToList();

            queryResult.AvailableLession = GetLession();
            queryResult.SelectedLession = CoachLessionResult;

            return View("CoachEdit", queryResult);

          

            
        }

        [Authentication]
        public IActionResult CEdit(string EAccount)
        {
            string eAccount = HttpContext.Session.GetString("Manager") ?? HttpContext.Session.GetString("Coach");

            if (eAccount == "Manager")
            {
                return Redirect("/Manage/Index");
            }

            var queryResult = (from e in _context.Employees
                               where e.EAccount == EAccount
                               select new EmployeeViewModel()
                               {
                                   ENumber = e.ENumber,
                                   EName = e.EName,
                                   EEngName = e.EEngName,
                                   EMail = e.EMail,
                                   EGender = e.EGender,
                                   EBirth = e.EBirth,
                                   EIdentityNumber = e.EIdentityNumber,
                                   EPhone = e.EPhone,
                                   EPhoto = e.EPhoto,
                                   EAccount = e.EAccount,
                                   EContactor = e.EContactor,
                                   EContactorPhone = e.EContactorPhone,
                                   EAddress = e.EAddress,
                                   EEnrollDate = e.EEnrollDate,
                                   EResignDate = e.EResignDate,
                                   EIsCoach = e.EIsCoach,
                                   EExplain = e.EExplain
                               }).FirstOrDefault();



            var employeeLessionResult = (from e in _context.Employees
                                         join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                         from cs in esc.DefaultIfEmpty()
                                         where e.EAccount == EAccount
                                         select cs.LcId.ToString()
                               ).ToList();

            queryResult.AvailableLession = GetLession();
            queryResult.SelectedLession = employeeLessionResult;

            return View("CoachEdit", queryResult);

        }

        [HttpPost]
        [Authentication]
        public async Task<IActionResult> CEdit(EmployeeViewModel employee, [FromForm(Name = "EPhoto")] IFormFile EPhoto)
        {
            string eAccount = HttpContext.Session.GetString("Manager") ?? HttpContext.Session.GetString("Coach");

            if (eAccount == "Manager")
            {
                return Redirect("/Manage/Index");
            }

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    if (EPhoto != null && EPhoto.Length > 0)
                    {


                        using (var ms = new MemoryStream())
                        {
                            EPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            employee.EPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }
                    else
                    {
                        var LessionResult = (from e in _context.Employees
                                             join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                             from cs in esc.DefaultIfEmpty()
                                             where e.ENumber == employee.ENumber
                                             select cs.LcId.ToString()
                                                ).ToList();

                        employee.AvailableLession = GetLession();
                        employee.SelectedLession = LessionResult;
                        var user = await _context.Employees.FirstOrDefaultAsync(u => u.EAccount == employee.EAccount);
                        employee.EPhoto = user.EPhoto;
                    }
                    return View("CoachEdit", employee);
                }
                else
                {

                    var employeeResult = await _context.Employees.FirstOrDefaultAsync(u => u.ENumber == employee.ENumber);

                    employeeResult.EName = employee.EName;
                    employeeResult.EEngName = employee.EEngName;
                    employeeResult.EGender = (bool)employee.EGender;
                    employeeResult.EBirth = (DateTime)employee.EBirth;
                    employeeResult.EIdentityNumber = employee.EIdentityNumber;
                    employeeResult.EPhone = employee.EPhone;
                    employeeResult.EMail = employee.EMail;
                    employeeResult.EAccount = employee.EAccount;
                    employeeResult.EContactor = employee.EContactor;
                    employeeResult.EContactorPhone = employee.EContactorPhone;
                    employeeResult.EAddress = employee.EAddress;
                    employeeResult.EEnrollDate = employee.EEnrollDate;
                    employeeResult.EResignDate = employee.EResignDate;
                    employeeResult.EIsCoach = (bool)employee.EIsCoach;
                    employeeResult.EExplain = employee.EExplain;
                    employee.EPhoto = employeeResult.EPhoto;

                    //若密碼重設欄有字串表示要重設密碼 
                    if (!string.IsNullOrEmpty(employee.EPassword))
                    {
                        // 轉換 password -> sha2_256 比較
                        byte[] data = Encoding.GetEncoding(1252).GetBytes(employee.EPassword);
                        var sha = new SHA256Managed();
                        byte[] bytesEncode = sha.ComputeHash(data);
                        employeeResult.EPassword = bytesEncode;
                    }

                    if (EPhoto != null && EPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            EPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            employeeResult.EPhoto = Convert.ToBase64String(fileBytes);
                            employee.EPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }

                    if (employeeResult != null)
                    {
                        await _context.SaveChangesAsync();
                    }



                    var oldCoachSpecials = (from e in _context.Employees
                                            join cs in _context.CoachSpecials on e.EId equals cs.EId into esc
                                            from cs in esc.DefaultIfEmpty()
                                            where e.ENumber == employee.ENumber
                                            select cs
                                   ).ToList();

                    //移除一個List
                    _context.CoachSpecials.RemoveRange(oldCoachSpecials);
                    _context.SaveChanges();


                    var NewCoachSpecials = new List<CoachSpecial>();

                    //增加被勾選的課程
                    foreach (var item in employee.SelectedLession)
                    {
                        NewCoachSpecials.Add(new CoachSpecial() { EId = employeeResult.EId, LcId = int.Parse(item) });
                    }

                    _context.CoachSpecials.AddRange(NewCoachSpecials);
                    _context.SaveChanges();

                    ViewData["success"] = "修改成功!";

                    employee.AvailableLession = GetLession();
                    return View("CoachEdit", employee);
                }
            }


        }




        [Authentication]
        public IActionResult CoachPasswordEdit()
        {
            string eAccount = HttpContext.Session.GetString("Manager") ?? HttpContext.Session.GetString("Coach");

            if (eAccount == "Manager")
            {
                return Redirect("/Manage/Index");
            }

            
            return View();
        }

        [HttpPost]
        [Authentication]
        public async Task<IActionResult> CoachPasswordEdit(EditPasswordViewModel password)
        {
            string eAccount = HttpContext.Session.GetString("Manager") ?? HttpContext.Session.GetString("Coach");

            if (eAccount == "Manager")
            {
                return Redirect("/Manage/Index");
            }

            if (ModelState.IsValid)
            {
                // 轉換 password -> sha2_256 比較 (轉使用者輸入的舊密碼與資料庫比較)
                byte[] data = Encoding.GetEncoding(1252).GetBytes(password.OldPassword);
                var sha = new SHA256Managed();
                byte[] bytesEncode = sha.ComputeHash(data);


                var query = await _context.Employees.FirstOrDefaultAsync(
                                   x => x.EPassword == bytesEncode &&
                                        x.EAccount == eAccount);


                bool isDataError = false;

                if (query == null)
                {
                    ViewData["errorOld"] = "舊密碼輸入錯誤!";
                    isDataError = true;
                }
                if (password.NewPassword != password.CheckPassword)
                {
                    ViewData["errorNew"] = "新密碼與確認密碼不符!";
                    isDataError = true;
                }
                if (password.OldPassword == password.NewPassword)
                {
                    ViewData["errorDouble"] = "舊密碼與新密碼不可以一樣!";
                    isDataError = true;
                }

                if (isDataError)
                {
                    return View("CoachPasswordEdit");

                }

                // 轉換 password -> sha2_256 比較  (轉新密碼存進資料庫)
                data = Encoding.GetEncoding(1252).GetBytes(password.NewPassword);
                bytesEncode = sha.ComputeHash(data);
                query.EPassword = bytesEncode;
                await _context.SaveChangesAsync();

                ViewData["success"] = "密碼修改成功!";

                return View("CoachPasswordEdit", query);
            }
            else
            {
                
                return View("CoachPasswordEdit");
            }
            
            
        }

        public IActionResult KnowledgeColumn()
        {
            var viewModel = _context.KnowledgeColumns.ToList();

            return View(viewModel);
        }

		//--------------------------------------------------------- 君 START ------------------------------------------------------------
		public IActionResult ShopMemberList()
		{
			return View();
		}
		public IActionResult SignupClass1()
		{
			return View();
		}
		public IActionResult SignupClass2(int id)
		{
			ViewData["CID"] = id;
			return View();
			//return Content(cid.ToString());
		}

		public IActionResult AddClassManage1(string cName, int lcID, int tID, int cTotalLession, int eID,
																		int cExcept, int cprice)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(cName) || string.IsNullOrEmpty(lcID.ToString()) ||
					string.IsNullOrEmpty(tID.ToString()) || string.IsNullOrEmpty(cTotalLession.ToString()) ||
					string.IsNullOrEmpty(eID.ToString()) ||
					string.IsNullOrEmpty(cExcept.ToString()) || string.IsNullOrEmpty(cprice.ToString()))
				{
					ViewData["error"] = "有未輸入的欄位，請檢查後再傳送！";
					return View("AddClassManage1");
				}

				//取到並存下 Session
				HttpContext.Session.SetString("cName", cName);
				HttpContext.Session.SetString("lcID", lcID.ToString());
				HttpContext.Session.SetString("tID", tID.ToString());
				HttpContext.Session.SetString("cTotalLession", cTotalLession.ToString());
				HttpContext.Session.SetString("eID", eID.ToString());
				HttpContext.Session.SetString("cExcept", cExcept.ToString());
				HttpContext.Session.SetString("cprice", cprice.ToString());
			}
			return View("AddClassManage2");
		}

		public IActionResult AddClassManage2()
		{
			return View();
		}

		public IActionResult QueryRoomManage()
		{
			return View();
		}

		public IActionResult QueryClassRecord()
		{
			return View();
		}

		//---------------------------------------------------------  君 END  ------------------------------------------------------------

	}
}
