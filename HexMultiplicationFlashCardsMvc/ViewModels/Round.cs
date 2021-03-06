﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    public class Round
    {
        //hex numbers
        public string Num { get; set; }

        //navigation
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<FlashCard> Questions { get; set; }
    }
}