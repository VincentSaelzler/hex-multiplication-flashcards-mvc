using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class RoundsController : Controller
    {
        // GET: Rounds
        public ActionResult Index()
        {
            return View();
        }

        // GET: Rounds/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rounds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rounds/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rounds/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rounds/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rounds/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rounds/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
