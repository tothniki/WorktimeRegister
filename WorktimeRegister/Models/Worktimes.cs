﻿using System;
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
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}",  ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public DateTime? Arrival { get; set; }
        public DateTime? Leaving { get; set; }

        //Because the N-1 relationship:
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}