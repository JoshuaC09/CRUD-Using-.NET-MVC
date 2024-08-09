using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentsController(ApplicationDbContext dbContext)
        {
            _dbContext= dbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel studentViewModel)
        {
            var student = new Student
            {
                Name = studentViewModel.Name,
                Email = studentViewModel.Email,
                Phone = studentViewModel.Phone,
                isSubScribed = studentViewModel.isSubScribed
            };

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _dbContext.Students.ToListAsync();
            return View(students);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
            var student = await _dbContext.Students.FindAsync(id);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> edit(Student studentViewModel)
        {
            var student = await _dbContext.Students.FindAsync(studentViewModel.Id);

            if(student is not null)
            {
                student.Name = studentViewModel.Name;
                student.Email = studentViewModel.Email;
                student.Phone = studentViewModel.Phone;
                student.isSubScribed    = studentViewModel.isSubScribed;

                await _dbContext.SaveChangesAsync();

            }
            return RedirectToAction("List", "Students"); 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student studentViewModel)
        {
            var student = await _dbContext.Students.AsNoTracking()
                                                   .FirstOrDefaultAsync(item => item.Id ==studentViewModel.Id);

            if(student is not null)
            {
                _dbContext.Students.Remove(studentViewModel);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }

        
    }
}   
