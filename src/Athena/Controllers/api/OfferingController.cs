using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers.api
{
    public class OfferingController : Controller
    {
        // GET: Offering
        public ActionResult Index()
        {
            return View();
        }

        // GET: Offering/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Offering/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offering/Create
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

        // GET: Offering/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Offering/Edit/5
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

        // GET: Offering/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Offering/Delete/5
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