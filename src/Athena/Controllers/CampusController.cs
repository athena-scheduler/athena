using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    public class CampusController : Controller
    {
        // GET: Campus
        public ActionResult Index()
        {
            return View();
        }

        // GET: Campus/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Campus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Campus/Create
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

        // GET: Campus/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Campus/Edit/5
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

        // GET: Campus/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Campus/Delete/5
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