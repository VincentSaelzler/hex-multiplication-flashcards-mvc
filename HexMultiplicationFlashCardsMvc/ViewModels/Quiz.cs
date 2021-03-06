﻿using System;
using System.Collections.Generic;

namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    public class Quiz
    {
        public string Description { get; set; }
        //TODO: stop displaying the "Started" field in edit template - just use datetime.now and same for finished
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }

        //hex numbers
        public string MinMultiplier { get; set; }
        public string MinMultiplicand { get; set; }
        public string MaxMultiplier { get; set; }
        public string MaxMultiplicand { get; set; }

        public int Id { get; set; }
        //public int? PersonId { get; set; }
        //public virtual Student Student { get; set; }
        public ICollection<Round> Rounds { get; set; }
    }
}