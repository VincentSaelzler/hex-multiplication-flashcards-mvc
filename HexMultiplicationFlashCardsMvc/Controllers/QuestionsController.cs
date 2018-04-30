﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HexMultiplicationFlashCardsMvc.Controllers
{
    public class QuestionsController : Controller
    {
        private DAL.FlashCardEntities db = new DAL.FlashCardEntities();
        
        // GET: Questions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Questions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DAL.Question questionDb = await db.Question.FindAsync(id);
            if (questionDb == null)
            {
                return HttpNotFound();
            }
            var questionVm = Mapper.Map<DAL.Question, ViewModels.FlashCard>(questionDb);
            return View(questionVm);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ViewModels.FlashCard flashCardVm)
        {
            if (ModelState.IsValid)
            {
                //TODO: document the pitfalls of direct AutoMapper => DAL Object Mapping
                //This is an awful idea! The view model doesn't have a round, so the code sets
                //questionDb.Round == null in the context, even though questionDb.RoundId keeps the correct value!
                //related entities are not loaded when using AutoMapper to DAL object
                //var questionDb = Mapper.Map<ViewModels.FlashCard, DAL.Question>(flashCardVm);

                //TODO: check model state

                var questionDb = await db.Question.FindAsync(flashCardVm.Id);

                questionDb.Response = flashCardVm.Response;
                db.Entry(questionDb).State = EntityState.Modified;
                await db.SaveChangesAsync();
               
                var quizId = questionDb.Round.Quiz.Id;
                return RedirectToAction("Take", "Quizzes", new { id = quizId });
            }

            return View(flashCardVm);
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
