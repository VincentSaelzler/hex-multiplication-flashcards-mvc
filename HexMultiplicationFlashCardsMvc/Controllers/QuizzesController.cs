using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
//using HexMultiplicationFlashCardsMvc.DAL;
//using HexMultiplicationFlashCardsMvc.ViewModels;

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class QuizzesController : Controller
    {
        private DAL.FlashCardEntities db = new DAL.FlashCardEntities();

        //// GET: Quizzes
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.Quizs.ToListAsync());
        //}

        //// GET: Quizzes/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Quiz quiz = await db.Quizs.FindAsync(id);
        //    if (quiz == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(quiz);
        //}

        // GET: Quizzes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ViewModels.Quiz quizVm)
        //TODO: implement over posting attack //public async Task<ActionResult> Create([Bind(Include = "Id,Description,Started,Finished")] ViewModels.Quiz quiz)
        {

            //TODO: handle pasing errors
            int MinMultiplier = int.Parse(quizVm.MinMultiplier, System.Globalization.NumberStyles.HexNumber);
            int MinMultiplicand = int.Parse(quizVm.MinMultiplicand, System.Globalization.NumberStyles.HexNumber);
            int MaxMultiplier = int.Parse(quizVm.MaxMultiplier, System.Globalization.NumberStyles.HexNumber);
            int MaxMultiplicand = int.Parse(quizVm.MaxMultiplicand, System.Globalization.NumberStyles.HexNumber);

            if (ModelState.IsValid)
            {
                //TESTING
                quizVm.Started = DateTime.Now;

                //add questions
                var questionsVm = new List<ViewModels.FlashCard>();
                for (int multiplier = MinMultiplier; multiplier <= MaxMultiplier; multiplier++)
                {
                    for (int multiplicand = MinMultiplicand; multiplicand <= MaxMultiplicand; multiplicand++)
                    {
                        var questionVm = new ViewModels.FlashCard(multiplier, multiplicand);
                        questionsVm.Add(questionVm);
                    }
                }
                

                //add round
                var roundVm = new ViewModels.Round();
                roundVm.Questions = questionsVm;
                quizVm.Rounds = new ViewModels.Round[] { roundVm };
                //add quiz                
                var quizDb = Mapper.Map<ViewModels.Quiz, DAL.Quiz>(quizVm);
                db.Quiz.Add(quizDb);
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }



            return View(quizVm);
        }

        //// GET: Quizzes/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Quiz quiz = await db.Quizs.FindAsync(id);
        //    if (quiz == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(quiz);
        //}

        //// POST: Quizzes/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Started,Finished")] Quiz quiz)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(quiz).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(quiz);
        //}

        //// GET: Quizzes/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Quiz quiz = await db.Quizs.FindAsync(id);
        //    if (quiz == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(quiz);
        //}

        //// POST: Quizzes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Quiz quiz = await db.Quizs.FindAsync(id);
        //    db.Quizs.Remove(quiz);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
