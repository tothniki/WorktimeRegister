using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    public class Worktimes
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}",  ApplyFormatInEditMode = true)]
        [DataType("Date")]
        public DateTime Date { get; set; }

        [DataType("Time")]
        public DateTime? Arrival { get; set; }

        [DataType("Time")]
        public DateTime? Leaving { get; set; }

        //Because the N-1 relationship:
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}