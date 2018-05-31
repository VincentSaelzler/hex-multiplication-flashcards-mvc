using AutoMapper;
using HexMultiplicationFlashCardsMvc.Extensions;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
//using HexMultiplicationFlashCardsMvc.DAL;
//using HexMultiplicationFlashCardsMvc.ViewModels;
//TODO: explore the automapper methods that project source objects to target objects without pulling
//un-necessary fields.

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class QuizzesController : Controller
    {
        private DAL.FlashCardEntities db = new DAL.FlashCardEntities();

        // GET: Quizzes
        public async Task<ActionResult> Index()
        {
            var dbQuizzes = await db.Quiz.ToListAsync();
            var vmQuizzes = dbQuizzes.Select(q => Mapper.Map<DAL.Quiz, ViewModels.Quiz>(q));
            return View(vmQuizzes);
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

            var quizVm = Mapper.Map<DAL.Quiz, ViewModels.QuizDetails>(quizDb);
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
        public async Task<ActionResult> Create(ViewModels.Quiz vmQuiz)
        //TODO: implement over posting attack //public async Task<ActionResult> Create([Bind(Include = "Id,Description,Started,Finished")] ViewModels.Quiz quiz)
        {
            //TODO: handle pasing errors
            //TODO: create quiz domain model?
            int MinMultiplier = vmQuiz.MinMultiplier.ParseHex();
            int MinMultiplicand = vmQuiz.MinMultiplicand.ParseHex();
            int MaxMultiplier = vmQuiz.MaxMultiplier.ParseHex();
            int MaxMultiplicand = vmQuiz.MaxMultiplicand.ParseHex();

            if (ModelState.IsValid)
            {
                //https://stackoverflow.com/questions/7311949/ramifications-of-dbset-create-versus-new-entity
                //PERFECTION: experiment then add to lessons learned; will db.Quiz.Create() avoid having nulls in
                //Quiz.Round unlike new Quiz()?
                var dbQuiz = db.Quiz.Create();
                var dbRound = db.Round.Create();

                dbQuiz.Description = vmQuiz.Description;
                dbQuiz.Started = DateTime.Now;
                dbQuiz.Round.Add(dbRound);

                //create questions
                for (int multiplier = MinMultiplier; multiplier <= MaxMultiplier; multiplier++)
                {
                    for (int multiplicand = MinMultiplicand; multiplicand <= MaxMultiplicand; multiplicand++)
                    {
                        var mQuestion = new Models.Question(multiplier, multiplicand);

                        dbRound.Question.Add(
                            new DAL.Question()
                            {
                                Multiplicand = mQuestion.Multiplicand,
                                Multiplier = mQuestion.Multiplier,
                                Product = mQuestion.Product
                            });
                    }
                }

                db.Quiz.Add(dbQuiz);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vmQuiz);
        }

        // GET: Quizzes/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

        //PERFECTION: make naming conventions consistent (e.g. dbRound vs roundDb

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Started,Finished")] Quiz quiz)
        public async Task<ActionResult> Edit(ViewModels.Quiz vmQuiz)
        {
            if (ModelState.IsValid)
            {
                var dbQuiz = await db.Quiz.FindAsync(vmQuiz.Id);
                dbQuiz.Description = vmQuiz.Description;
                db.Entry(dbQuiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vmQuiz);
        }

        // GET: Quizzes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbQuiz = await db.Quiz.FindAsync(id);
            if (dbQuiz == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<DAL.Quiz, ViewModels.Quiz>(dbQuiz));
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var dbQuiz = await db.Quiz.FindAsync(id);
            db.Quiz.Remove(dbQuiz);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //TODO: document lessons learned about copying by reference vs by value
        //specifically when copying old questions from last round to new round
        public async Task<ActionResult> Take(int? id)
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
            int? questionId = await GetNextQuestionId(quizDb);
            if (questionId == null)
            {
                return RedirectToAction("Details", new { id });
            }
            return RedirectToAction("Edit", "Questions", new { id = questionId });
        }

        private async Task<int?> GetNextQuestionId(DAL.Quiz quiz)
        {
            //PERFECTION: eager load all entities?
            //PERFECTION: display multiplicand before multiplier?

            //this quiz is already complete
            if (quiz.Round.Any(r => r.Question.All(qst => qst.Response == qst.Product)))
            {
                //prevent the repeatedly overwriting the finish time every time take is called
                if (quiz.Finished == null)
                {
                    quiz.Finished = DateTime.Now;
                    await db.SaveChangesAsync();
                }
                return null;
            }
            //a round is currently in progress
            else if (quiz.Round.Any(r => r.Question.Any(qst => qst.Response == null)))
            {
                var round = quiz.Round.Single(r => r.Question.Any(qst => qst.Response == null));
                var questions = round.Question.Where(qst => qst.Response == null);
                //TODO: randomize
                int questionId = questions.First().Id;
                return questionId;
            }
            //make a new round
            else
            {
                //get old info
                var oldRound = quiz.Round.OrderByDescending(r => r.Num).First();
                var oldQuestions = oldRound.Question.Where(qst => qst.Response != qst.Product);

                //create new questions and round
                var questions = oldQuestions
                    .Select(oq => new DAL.Question()
                    {
                        Product = oq.Product,
                        Multiplicand = oq.Multiplicand,
                        Multiplier = oq.Multiplier
                    });

                var round = new DAL.Round()
                {
                    Num = oldRound.Num + 1,
                    Quiz = quiz,
                    Question = questions.ToList() //cannot convert directly from IEnumerable to ICollection
                };

                //save to DB
                db.Round.Add(round);
                await db.SaveChangesAsync();

                //pull from DB
                //TODO: randomize
                int questionId = round.Question.First().Id;
                return questionId;
            }
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
