using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    public class InstitutionController : Controller
    {
        // GET: Institution
        public ActionResult Index()
        {
            return View();
        }

        // GET: Institution/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Institution/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Institution/Create
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

        // GET: Institution/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Institution/Edit/5
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

        // GET: Institution/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Institution/Delete/5
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