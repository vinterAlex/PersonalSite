using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class Movies
    {
        public int MovieID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        public int Rate { get; set; }
        public string Thumbnail { get; set; }

        
    }
}