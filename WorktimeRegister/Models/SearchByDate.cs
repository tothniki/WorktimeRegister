using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    public class SearchByDate
    {
        [RegularExpression(@"\d{4}", ErrorMessage="It must contain 4 number!")]
        public int? Year { get; set; }

        [RegularExpression(@"\d{2}", ErrorMessage = "It must contain 2 number!")]
        public int? Month { get; set; }

        [RegularExpression(@"\d{2}", ErrorMessage = "It must contain 1 or 2 number!")]
        public int? Day { get; set; }
    }
}