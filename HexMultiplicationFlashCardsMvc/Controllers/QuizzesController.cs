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

        // GET: Quizzes
        public async Task<ActionResult> Index()
        {
            var q1 = new DAL.Question { Id = 1, Multiplicand = 1, Multiplier = 2 };
            var q2 = new DAL.Question { Id = 2, Multiplicand = 2, Multiplier = 2 };
            var q3 = new DAL.Question { Id = 3, Multiplicand = 3, Multiplier = 2 };

            //var fc1 = Mapper.Map<DAL.Question, ViewModels.FlashCard>(q1);
            //var fc2 = Mapper.Map<DAL.Question, ViewModels.FlashCard>(q2);
            //var fc3 = Mapper.Map<DAL.Question, ViewModels.FlashCard>(q3);
            //ICollection<ViewModels.FlashCard> fcs = new ViewModels.FlashCard[] { fc1, fc2, fc3 };

            ICollection<DAL.Question> qs1 = new DAL.Question[] { q1, q2 };
            ICollection<DAL.Question> qs2 = new DAL.Question[] { q3 };

            var rDb1 = new DAL.Round { Id = 1, Num = 1, Question = qs1 };
            var rDb2 = new DAL.Round { Id = 2, Num = 2, Question = qs2 };

            foreach (var q in rDb1.Question)
            {
                q.Round = rDb1;
                q.RoundId = rDb1.Id;
            }


            foreach (var q in rDb2.Question)
            {
                q.Round = rDb2;
                q.RoundId = rDb2.Id;
            }

            //var rVm1 = Mapper.Map<DAL.Round, ViewModels.Round>(rDb1);
            //var rVm2 = Mapper.Map<DAL.Round, ViewModels.Round>(rDb2);

            ICollection<DAL.Round> rs1 = new DAL.Round[] { rDb1, rDb2 };

            var qzDb = new DAL.Quiz() { Id = 1, Description = "Outer", Round = rs1 };

            foreach (var r in qzDb.Round)
            {
                r.Quiz = qzDb;
                r.QuizId = qzDb.Id;
            }

            var qzVm = Mapper.Map<DAL.Quiz, ViewModels.Quiz>(qzDb);

            //return View(await db.Quizs.ToListAsync());
            return View();
        }

        // GET: Quizzes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAL.Quiz quizDb = await db.Quiz.FindAsync(id);
            if (quizDb == null)
            {
                return HttpNotFound();
            }

            var quizVm = Mapper.Map<DAL.Quiz, ViewModels.Quiz>(quizDb);
            return View(quizVm);
        }

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
                var roundVm = new ViewModels.Round
                {
                    Questions = questionsVm
                };
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
