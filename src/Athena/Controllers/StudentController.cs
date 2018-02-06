using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athena.Core.Repositories;

namespace Athena.Controllers
{
    public class StudentController : Controller 
    {
        private readonly IStudentRepository _students;

        public StudentController(IStudentRepository students) =>
                _students = students ?? throw new ArgumentNullException(nameof(students));
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([FromBody] Student s )
        {
            try
            {
                // TODO: Add insert logic here
                await _students.AddAsync(s);
                Response.StatusCode = (int) HttpStatusCode.Created;
            }
            catch 
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Edit(Guid id, [FromBody] Student s)
        {
            if (id != s.Id)
            {
                //throw new 
            }
            await _students.EditAsync(s);
        }

        // POST: Student/Delete/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task Delete(Guid id) => await _students.DeleteAsync(id);
        
    }
}