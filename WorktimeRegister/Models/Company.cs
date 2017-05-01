using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    public class Company
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Company name")]
        public string Name { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Post code")]
        public int PostCode { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} length should be less than 50 characters long.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}