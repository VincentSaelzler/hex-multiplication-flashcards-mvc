using System;
using System.Collections.Generic;

namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    public class QuizDetails
    {
        public string Description { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }

        //hex numbers
        public string MinMultiplier { get; set; }
        public string MinMultiplicand { get; set; }
        public string MaxMultiplier { get; set; }
        public string MaxMultiplicand { get; set; }
        public string NumRounds { get; set; }

        public int Id { get; set; }
        //public int? PersonId { get; set; }
        //public virtual Student Student { get; set; }
        public IEnumerable<Round> Rounds { get; set; }
    }
}