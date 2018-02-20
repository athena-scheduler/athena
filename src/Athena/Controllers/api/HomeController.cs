using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    public class HomeController : Controller
    {
        // GET: HomeAPI
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeAPI/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeAPI/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeAPI/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeAPI/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeAPI/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeAPI/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeAPI/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}