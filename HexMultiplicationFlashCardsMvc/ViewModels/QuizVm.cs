using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HexMultiplicationFlashCardsMvc.ViewModels
{
    public class QuizVm
    {
        //"basic" properties
        public int Id { get; set; }
        public string Description { get; set; }
        public System.DateTime Started { get; set; } = DateTime.Now; //Default value is not in DAL model
        public Nullable<System.DateTime> Finished { get; set; }
        public Nullable<int> PersonId { get; set; }

        //navigation properties
        public DAL.Student Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<DAL.Round> Round { get; set; }

        //additional properties not in the DAL model
        [Required]
        public int? MinMultiplier { get; set; } = 3;
        [Required]
        public int? MaxMultiplier { get; set; } = 5;
        [Required]
        public int? MinMultiplicand { get; set; } = 3;
        [Required]
        public int? MaxMultiplicand { get; set; } = 5;
    }
}