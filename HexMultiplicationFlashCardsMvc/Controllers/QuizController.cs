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
using HexMultiplicationFlashCardsMvc.ViewModels;

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class QuizController : Controller
    {
        private FlashCardEntities db = new FlashCardEntities();

        // GET: Quiz
        public async Task<ActionResult> Index()
        {
            var quiz = db.Quiz.Include(q => q.Student);
            return View(await quiz.ToListAsync());
        }

        // GET: Quiz/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = await db.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: Quiz/Create
        public ActionResult Create()
        {
            ViewBag.PersonId = new SelectList(db.Student, "Id", "Name");
            QuizVm quizVm = new QuizVm(); //get default date value
            return View(quizVm);
        }

        // POST: Quiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //TODO: personID should be based on logged in user, never passed in as a parameter
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Started,Finished,PersonId,MinMultiplier,MaxMultiplier,MinMultiplicand,MaxMultiplicand")] QuizVm quizVm)
        {
            if (ModelState.IsValid)
            {
                //add questions

                //add round

                //add quiz
                Quiz quiz = AutoMapper.Mapper.Map<QuizVm, Quiz>(quizVm);
                db.Quiz.Add(quiz);
                await db.SaveChangesAsync();

                //redirect to quiz (round)
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.Student, "Id", "Name", quizVm.PersonId);
            return View(quizVm);
        }

        // GET: Quiz/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = await db.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.Student, "Id", "Name", quiz.PersonId);
            return View(quiz);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Started,Finished,PersonId")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.Student, "Id", "Name", quiz.PersonId);
            return View(quiz);
        }

        // GET: Quiz/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = await db.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Quiz quiz = await db.Quiz.FindAsync(id);
            db.Quiz.Remove(quiz);
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
