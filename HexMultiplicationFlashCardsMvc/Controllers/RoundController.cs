using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HexMultiplicationFlashCardsMvc.DAL;

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class RoundController : Controller
    {
        private FlashCardEntities db = new FlashCardEntities();

        // GET: Round
        public async Task<ActionResult> Index()
        {
            var round = db.Round.Include(r => r.Quiz);
            return View(await round.ToListAsync());
        }

        // GET: Round/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = await db.Round.FindAsync(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // GET: Round/Create
        public ActionResult Create()
        {
            ViewBag.QuizId = new SelectList(db.Quiz, "Id", "Id");
            return View();
        }

        // POST: Round/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Num,QuizId")] Round round)
        {
            if (ModelState.IsValid)
            {
                db.Round.Add(round);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.QuizId = new SelectList(db.Quiz, "Id", "Id", round.QuizId);
            return View(round);
        }

        // GET: Round/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = await db.Round.FindAsync(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuizId = new SelectList(db.Quiz, "Id", "Id", round.QuizId);
            return View(round);
        }

        // POST: Round/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Num,QuizId")] Round round)
        {
            if (ModelState.IsValid)
            {
                db.Entry(round).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.QuizId = new SelectList(db.Quiz, "Id", "Id", round.QuizId);
            return View(round);
        }

        // GET: Round/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = await db.Round.FindAsync(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // POST: Round/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Round round = await db.Round.FindAsync(id);
            db.Round.Remove(round);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
