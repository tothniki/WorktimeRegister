﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name="Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        [DataType("Date")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name="Place of Birth")]
        public string PlaceOfBirth { get; set; }

        //Because the 1-N relationship:
        public virtual ICollection<Worktimes> Worktimes { get; set; }
    }
}