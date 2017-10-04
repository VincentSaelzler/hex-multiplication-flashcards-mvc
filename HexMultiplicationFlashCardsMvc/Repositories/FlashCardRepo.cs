using AutoMapper;
using HexMultiplicationFlashCardsMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

//PERFECTION: in an environment with separation of responsibility
//this would all be programmed against an IFlashCardRepo interface
namespace HexMultiplicationFlashCardsMvc.Repositories
{
    static class FlashCardRepo
    {
        //CREATE
        public static int NewStudent()
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //TODO: actually select student
                //PERFECTION: take these out of the using?
                var s = new DAL.Student { Name = "Vince" };
                db.Student.Add(s);
                db.SaveChanges();
                return s.Id;
            }
        }
        public static int NewQuiz(int studentId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //PERFECTION: take these out of the using?
                var student = db.Student.Single(s => s.Id == studentId);
                var q = new DAL.Quiz { Started = DateTime.Now };
                student.Quiz.Add(q);

                db.SaveChanges();

                return q.Id;
            }
        }
        public static int NewRound(int quizId, int roundNum)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var quiz = db.Quiz.Single(q => q.Id == quizId);
                var round = new DAL.Round() { Num = roundNum };
                quiz.Round.Add(round);
                db.SaveChanges();
                return round.Id;
            }
        }
        public static void NewQuestions(int roundId, IEnumerable<FlashCard> flashCards)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var round = db.Round.Single(r => r.Id == roundId);
                IList<DAL.Question> questions = new List<DAL.Question>();

                foreach (var fc in flashCards)
                {
                    var question = Mapper.Map<FlashCard, DAL.Question>(fc);
                    question.Round = round;
                    db.Question.Add(question);
                }
                db.SaveChanges();
            }
        }

        //READ
        //PERFECTION: view model reflects FKs to map relationship between quizzes, rounds, questions
        public static int NumQuizQuestions(int quizId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var quiz = db.Quiz.Single(q => q.Id == quizId);
                return quiz.Round.SelectMany(r => r.Question).Count();
            }
        }
        public static int NumQuizQuestionsCorrect(int quizId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var quiz = db.Quiz.Single(q => q.Id == quizId);
                return quiz.Round.SelectMany(r => r.Question).Where(q => q.Response == q.Product).Count();
            }
        }
        public static int NumQuizQuestionsWrong(int quizId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var quiz = db.Quiz.Single(q => q.Id == quizId);
                return quiz.Round.SelectMany(r => r.Question).Where(q => q.Response != q.Product).Count();
            }
        }
        public static int GetMaxRoundNum(int quizId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //broke a single linq query to a few lines to understand how null result sets are
                //converted to a non-nullable int
                //db.Database.Log = s => Console.WriteLine(s);
                var quiz = db.Quiz.Single(q => q.Id == quizId);
                var roundsish = quiz.Round.Select(r => r.Num).DefaultIfEmpty();
                var rm = roundsish.Max();
                return rm;
            }
        }
        public static int GetCurrentRoundNum(int roundId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var round = db.Round.Single(q => q.Id == roundId);
                return round.Num;
            }
        }
        public static int GetQuizId(int roundId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var round = db.Round.Single(q => q.Id == roundId);
                return round.Quiz.Id;
            }
        }
        public static IEnumerable<FlashCard> GetCards(int roundId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                IList<FlashCard> flashCards = new List<FlashCard>();
                var round = db.Round.Single(r => r.Id == roundId);
                foreach (var q in round.Question)
                {
                    flashCards.Add(Mapper.Map<DAL.Question, FlashCard>(q));
                }
                return flashCards;
            }
        }

        //UPDATE
        public static void SaveQuestions(IEnumerable<FlashCard> flashCards)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //PERFECTION: only modify the "response" field
                //by querying from the DB by ID
                var questions = flashCards.Select(fc => Mapper.Map<FlashCard, DAL.Question>(fc));
                foreach (var q in questions)
                {
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
    }
}
